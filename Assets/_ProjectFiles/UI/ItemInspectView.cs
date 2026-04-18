using TMPro;
using UnityEngine;

public class ItemInspectView : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Awake()
    {
        Hide();
    }

    public void Show(string description)
    {
        root.SetActive(true);
        descriptionText.text = description;
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}