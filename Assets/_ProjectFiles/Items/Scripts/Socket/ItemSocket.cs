using UnityEngine;

public class ItemSocket : BaseInteractable
{
    [SerializeField] private Transform itemPoint;

    private ItemPickupController pickupController;

    public ItemInteractable CurrentItem { get; private set; }
    public Transform ItemPoint => itemPoint;

    private void Awake()
    {
        promptText = "положить";
        interactionType = InteractionType.Press;

        pickupController = FindFirstObjectByType<ItemPickupController>();
    }

    public void SetItem(ItemInteractable item)
    {
        CurrentItem = item;

        if (item != null)
            item.SetCurrentSocket(this);
    }

    public void ClearItem()
    {
        if (CurrentItem != null)
            CurrentItem.SetCurrentSocket(null);

        CurrentItem = null;
    }

    public override InteractionInfo GetInteractionInfo()
    {
        if (pickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (CurrentItem != null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (pickupController.CanPlaceIntoSocket(this))
            return new InteractionInfo(true, "положить", InteractionType.Press);

        return new InteractionInfo(false, "", InteractionType.Press);
    }

    public override void BeginInteract()
    {
        if (pickupController == null)
            return;

        pickupController.TryPlaceHeldItemIntoSocket(this);
    }
}