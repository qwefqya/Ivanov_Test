public interface IInteractable
{
    InteractionInfo GetInteractionInfo();

    void BeginInteract();
    void UpdateInteract(float holdTime);
    void EndInteract();
}