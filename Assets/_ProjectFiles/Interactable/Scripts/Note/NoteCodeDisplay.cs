using UnityEngine;

public class NoteCodeDisplay : MonoBehaviour
{
    [SerializeField] private GameObject codeVisualRoot;

    private void Awake()
    {
        HideCode();
    }

    public void ShowCode()
    {
        if (codeVisualRoot != null)
            codeVisualRoot.SetActive(true);
    }

    public void HideCode()
    {
        if (codeVisualRoot != null)
            codeVisualRoot.SetActive(false);
    }
}