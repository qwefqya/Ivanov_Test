using UnityEngine;

public class TestPressInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "взаимодействовать";

    public InteractionInfo GetInteractionInfo()
    {
        return new InteractionInfo(true, promptText, InteractionType.Press);
    }

    public void BeginInteract()
    {
        Debug.Log("Press begin: " + gameObject.name);
    }

    public void UpdateInteract(float holdTime)
    {
    }

    public void EndInteract()
    {
    }
}