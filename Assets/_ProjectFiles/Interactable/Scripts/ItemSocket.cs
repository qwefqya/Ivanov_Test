using UnityEngine;

public class ItemSocket : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform itemPoint;

    private ItemPickupController pickupController;

    public ItemInteractable CurrentItem { get; private set; }
    public Transform ItemPoint => itemPoint;

    private void Awake()
    {
        pickupController = FindFirstObjectByType<ItemPickupController>();
    }

    public void SetItem(ItemInteractable item)
    {
        CurrentItem = item;
    }

    public void ClearItem()
    {
        CurrentItem = null;
    }

    public InteractionInfo GetInteractionInfo()
    {
        if (pickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (CurrentItem != null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (pickupController.CanPlaceIntoSocket(this))
        {
            bool isHomeSocket =
                pickupController.CurrentHeldItem != null &&
                pickupController.CurrentHeldItem.HomeSocket == this;

            return new InteractionInfo(true, isHomeSocket ? "вернуть" : "положить", InteractionType.Press);
        }

        return new InteractionInfo(false, "", InteractionType.Press);
    }

    public void BeginInteract()
    {
        if (pickupController == null)
            return;

        pickupController.TryPlaceHeldItemIntoSocket(this);
    }

    public void UpdateInteract(float holdTime) { }
    public void EndInteract() { }
}