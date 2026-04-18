using UnityEngine;

public class NotePresentation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string openTrigger = "Open";
    [SerializeField] private string closeTrigger = "Close";
    [SerializeField] private NoteCodeDisplay codeDisplay;

    public void PlayOpen()
    {
        if (animator != null)
            animator.SetTrigger(openTrigger);

        if (codeDisplay != null)
            codeDisplay.ShowCode();
    }

    public void PlayClose()
    {
        if (animator != null)
            animator.SetTrigger(closeTrigger);

        if (codeDisplay != null)
            codeDisplay.HideCode();
    }

    public void ForceClosedState()
    {
        if (animator != null)
        {
            animator.Play("Idle", 0, 0f);
            animator.Update(0f);
        }

        if (codeDisplay != null)
            codeDisplay.HideCode();
    }
}