using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;

    public enum State { Idle, TransitionOut, MiddleTransition, TransitionIn }

    public State SceneState => state;

    [Header("Settings")]
    [SerializeField, Range(0.1f, 0.5f)] private float transitionInInterval;

    public static event EventHandler<OnSceneTransitionLoadEventArgs> OnSceneTransitionOutStart;
    public static event EventHandler<OnSceneTransitionLoadEventArgs> OnSceneTransitionInStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoadStart;
    public static event EventHandler<OnSceneLoadEventArgs> OnSceneLoadComplete;

    private string sceneToLoad;

    public float LoadProgress { get; private set; }

    private const float SCENE_READY_PERCENT = 0.9f;
    private const float SCENE_READY_PAUSE_TIME = 0.5f;

    public class OnSceneLoadEventArgs : EventArgs
    {
        public string originScene;
        public string targetScene;
    }

    public class OnSceneTransitionLoadEventArgs : EventArgs
    {
        public string originScene;
        public string targetScene;
        public TransitionType transitionType;
    }

    private void OnEnable()
    {
        SceneTransitionUIHandler.OnTransitionOutEnd += TransitionUIHandler_OnTransitionOutEnd;
        SceneTransitionUIHandler.OnTransitionInEnd += TransitionUIHandler_OnTransitionInEnd;
    }

    private void OnDisable()
    {
        SceneTransitionUIHandler.OnTransitionOutEnd -= TransitionUIHandler_OnTransitionOutEnd;
        SceneTransitionUIHandler.OnTransitionInEnd -= TransitionUIHandler_OnTransitionInEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SimulateTransitionIn(TransitionType.Fade);
    }

    private void SimulateTransitionIn(TransitionType transitionType)
    {
        //Simulate the load of current scene, only to TransitionIn the first scene played
        OnSceneLoadComplete?.Invoke(this, new OnSceneLoadEventArgs { originScene = "", targetScene = SceneManager.GetActiveScene().name });
        OnSceneTransitionInStart.Invoke(this, new OnSceneTransitionLoadEventArgs { originScene = "", targetScene = SceneManager.GetActiveScene().name, transitionType = transitionType});
        SetSceneState(State.TransitionIn);
    }

    private void SimulateRegularLoad()
    {
        OnSceneLoadComplete?.Invoke(this, new OnSceneLoadEventArgs { originScene = "", targetScene = SceneManager.GetActiveScene().name });
        SetSceneState(State.Idle);
    }

    private void SetSceneState(State state) => this.state = state;
    private bool CanChangeScene() => state == State.Idle;
    private void SetSceneToLoad(string sceneToLoad) => this.sceneToLoad = sceneToLoad;
    private void ClearSceneToLoad() => sceneToLoad = "";


    #region SimpleLoad
    public void SimpleReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SimpleLoadTargetScene(currentSceneName);
    }

    public void SimpleLoadTargetScene(string targetScene)
    {
        if (!CanChangeScene()) return;
        StartCoroutine(LoadSceneAsyncCoroutine(targetScene));
    }
    #endregion

    #region TransitionLoad
    public void TransitionReloadCurrentScene(TransitionType transitionType)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        TransitionLoadTargetScene(currentSceneName, transitionType);
    }

    public void TransitionLoadTargetScene(string targetScene, TransitionType transitionType) //Transition Out Method
    {
        if (!CanChangeScene()) return;

        string originScene = SceneManager.GetActiveScene().name;

        SetSceneState(State.TransitionOut);
        OnSceneTransitionOutStart?.Invoke(this, new OnSceneTransitionLoadEventArgs { originScene = originScene, targetScene = targetScene, transitionType = transitionType });
        SetSceneToLoad(targetScene);
    }

    private IEnumerator LoadSceneTransitionInCoroutine(string targetScene, TransitionType transitionType) //Transition In Corroutine
    {
        string originScene = SceneManager.GetActiveScene().name;

        yield return StartCoroutine(LoadSceneAsyncCoroutine(targetScene)); //Async Scene Load happens here
        yield return new WaitForSeconds(transitionInInterval);

        OnSceneTransitionInStart?.Invoke(this, new OnSceneTransitionLoadEventArgs { originScene = originScene, targetScene = targetScene, transitionType = transitionType });

        SetSceneState(State.TransitionIn);
    }

    #endregion

    #region LoadScene

    public void LoadScene(string targetScene) //No Async
    {
        string originScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadSceneAsync(targetScene);
        OnSceneLoadComplete?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });
    }

    private IEnumerator LoadSceneAsyncCoroutine(string targetScene)
    {
        string originScene = SceneManager.GetActiveScene().name;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        asyncLoad.allowSceneActivation = false;
        LoadProgress = 0f;

        OnSceneLoadStart?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });

        while (!asyncLoad.isDone)
        {
            LoadProgress = Mathf.Clamp01(asyncLoad.progress / SCENE_READY_PERCENT);

            if(asyncLoad.progress >= SCENE_READY_PERCENT)
            {
                yield return new WaitForSeconds(SCENE_READY_PAUSE_TIME);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        OnSceneLoadComplete?.Invoke(this, new OnSceneLoadEventArgs { originScene = originScene, targetScene = targetScene });
    }

    #endregion

    public void QuitGame() => Application.Quit();

    #region TransitionUIHandler Subscriptions
    private void TransitionUIHandler_OnTransitionOutEnd(object sender, SceneTransitionUIHandler.OnTransitionUIEventArgs e)
    {
        SetSceneState(State.MiddleTransition);
        StartCoroutine(LoadSceneTransitionInCoroutine(sceneToLoad, e.transitionType));
    }
    private void TransitionUIHandler_OnTransitionInEnd(object sender, SceneTransitionUIHandler.OnTransitionUIEventArgs e)
    {
        ClearSceneToLoad();
        SetSceneState(State.Idle);
    }
    #endregion

}
