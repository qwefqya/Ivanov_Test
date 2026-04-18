using TMPro;
using UnityEngine;

public class InteractionPromptView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;

    private void Awake()
    {
        Hide();
    }

    // скрываем текст на старте

    public void Show(string text)
    {
        promptText.gameObject.SetActive(true);
        promptText.text = text;
    }

    //функция вызова теста

    public void Hide()
    {
        promptText.gameObject.SetActive(false);
    }

    //функция скрытия текста
}