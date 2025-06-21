using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TutorializedActionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float openTime;
    [SerializeField] private float closeTime;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    public static event EventHandler<OnTutorializedActionEventArgs> OnTutorializedActionUIOpen;
    public static event EventHandler<OnTutorializedActionEventArgs> OnTutorializedActionUIClose;

    protected bool isActive = false;

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

    public abstract TutorializedAction GetTutorializedAction();

    private void TutorialOpeningManager_OnTutorializedActionOpen(object sender, TutorialOpeningManager.OnTutorializedActionEventArgs e)
    {
        if (e.tutorializedAction != GetTutorializedAction()) return;
        OpenTutorializedAction();
    }
}
