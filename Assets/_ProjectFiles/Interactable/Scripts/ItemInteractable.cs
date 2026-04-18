using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition definition;

    private ItemPickupController pickupController;

    public ItemDefinition Definition => definition;
    public ItemState State { get; private set; } = ItemState.InWorld;

    private void Awake()
    {
        pickupController = FindFirstObjectByType<ItemPickupController>();
    }

    public void SetState(ItemState state)
    {
        State = state;
    }

    public InteractionInfo GetInteractionInfo()
    {
        if (pickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (pickupController.HasHeldItem && pickupController.CurrentHeldItem != this)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (State == ItemState.InWorld || State == ItemState.InSocket)
            return new InteractionInfo(true, "поднять", InteractionType.Press);

        if (State == ItemState.Inspecting)
            return new InteractionInfo(true, "взять", InteractionType.Press);

        return new InteractionInfo(false, "", InteractionType.Press);
    }

    public void BeginInteract()
    {
        if (pickupController == null)
            return;

        pickupController.TryInteractWithItem(this);
    }

    public void UpdateInteract(float holdTime)
    {
    }

    public void EndInteract()
    {
    }
}