using UnityEngine;

public class NotePresentation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string openTrigger = "Open";
    [SerializeField] private string closeTrigger = "Close";

    public void PlayOpen()
    {
        if (animator != null)
            animator.SetTrigger(openTrigger);
    }

    public void PlayClose()
    {
        if (animator != null)
            animator.SetTrigger(closeTrigger);
    }

    public void ForceClosedState()
    {
        if (animator == null)
            return;

        animator.Play("Idle", 0, 0f);
        animator.Update(0f);
    }
}