using UnityEngine;

public class NotePresentation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string openTrigger = "Open";

    public void PlayOpen()
    {
        if (animator != null)
            animator.SetTrigger(openTrigger);
    }
}