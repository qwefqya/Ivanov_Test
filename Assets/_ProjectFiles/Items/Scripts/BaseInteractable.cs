using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [Header("Base Interaction")]
    [SerializeField] protected string promptText = "";
    [SerializeField] protected InteractionType interactionType = InteractionType.Press;

    public virtual InteractionInfo GetInteractionInfo()
    {
        return new InteractionInfo(CanInteract(), promptText, interactionType);
    }

    protected virtual bool CanInteract()
    {
        return true;
    }

    public abstract void BeginInteract();

    public virtual void UpdateInteract(float holdTime)
    {
    }

    public virtual void EndInteract()
    {
    }
}