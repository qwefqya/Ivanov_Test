using TMPro;
using UnityEngine;

public class NoteCodeDisplay : MonoBehaviour
{
    [SerializeField] private GameObject codeVisualRoot;
    [SerializeField] private TextMeshPro codeText;
    [SerializeField] private HackTerminalInteractable sourceTerminal;

    private void Awake()
    {
        RefreshText();
        HideCode();
    }

    public void RefreshText()
    {
        if (codeText == null || sourceTerminal == null)
            return;

        string encoded = sourceTerminal.GetEncodedSequence();
        codeText.text = HackCodeUtility.ToSymbolString(encoded, true);
    }

    public void ShowCode()
    {
        RefreshText();

        if (codeVisualRoot != null)
            codeVisualRoot.SetActive(true);
    }

    public void HideCode()
    {
        if (codeVisualRoot != null)
            codeVisualRoot.SetActive(false);
    }
}