using UnityEngine;

public class TestHoldInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "крутить";

    public InteractionInfo GetInteractionInfo()
    {
        return new InteractionInfo(true, promptText, InteractionType.Hold);
    }

    public void BeginInteract()
    {
        Debug.Log("Hold begin: " + gameObject.name);
    }

    public void UpdateInteract(float holdTime)
    {
        Debug.Log($"Holding {gameObject.name}: {holdTime:F2}");
    }

    public void EndInteract()
    {
        Debug.Log("Hold end: " + gameObject.name);
    }
}