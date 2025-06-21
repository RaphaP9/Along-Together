using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TutorializedActionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField, Range (0f,2f)] private float openTime;
    [SerializeField, Range(0f, 2f)] private float closeTime;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    public static event EventHandler<OnTutorializedActionEventArgs> OnTutorializedActionUIOpen;
    public static event EventHandler<OnTutorializedActionEventArgs> OnTutorializedActionUIClose;

    protected bool isActive = false;
    protected bool tutorialConditionMet = false;

    public class OnTutorializedActionEventArgs : EventArgs
    {
        public TutorializedActionUI tutorializedActionUI;
    }

    protected virtual void OnEnable()
    {
        TutorialOpeningManager.OnTutorializedActionOpen += TutorialOpeningManager_OnTutorializedActionOpen;
    }

    protected virtual void OnDisable()
    {
        TutorialOpeningManager.OnTutorializedActionOpen -= TutorialOpeningManager_OnTutorializedActionOpen;
    }

    private void Update()
    {
        HandleUpdateConditionMeeting();
    }

    private void HandleUpdateConditionMeeting() //Some TutorializedActions Should Be Checked In Update (Ex. Distance Covered) if not, only override CheckCondition() to false
    {
        if (tutorialConditionMet) return;
        if (!CheckCondition()) return;

        CloseTutorializedAction();

        tutorialConditionMet = true;
    }

    #region Animations

    private void ShowUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }
    #endregion

    #region Open & Close Logic
    protected void OpenTutorializedAction()
    {
        StartCoroutine(OpenTutorializedActionCoroutine());
    }

    protected void CloseTutorializedAction()
    {
        StartCoroutine (CloseTutorializedActionCoroutine());
    }

    private IEnumerator OpenTutorializedActionCoroutine()
    {
        isActive = true;

        yield return new WaitForSeconds(openTime);

        OnTutorializedActionUIOpen?.Invoke(this, new OnTutorializedActionEventArgs { tutorializedActionUI = this });
        ShowUI();
    }

    private IEnumerator CloseTutorializedActionCoroutine()
    {
        yield return new WaitForSeconds (closeTime);

        HideUI();
        OnTutorializedActionUIClose?.Invoke(this, new OnTutorializedActionEventArgs { tutorializedActionUI = this });

        isActive = false;
    }
    #endregion

    #region Abstract Methods
    public abstract TutorializedAction GetTutorializedAction();
    protected abstract bool CheckCondition();
    #endregion

    private void TutorialOpeningManager_OnTutorializedActionOpen(object sender, TutorialOpeningManager.OnTutorializedActionEventArgs e)
    {
        if (e.tutorializedAction != GetTutorializedAction()) return;
        OpenTutorializedAction();
    }
}
