using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition definition;
    [SerializeField] private ItemSocket homeSocket;
    [SerializeField] private Collider itemCollider;

    private ItemPickupController pickupController;

    public ItemDefinition Definition => definition;
    public ItemSocket HomeSocket => homeSocket;
    public ItemState State { get; private set; } = ItemState.InWorld;

    private void Awake()
    {
        pickupController = FindFirstObjectByType<ItemPickupController>();

        if (itemCollider == null)
            itemCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        if (homeSocket != null)
        {
            State = ItemState.InSocket;
            homeSocket.SetItem(this);
        }
    }

    public void SetState(ItemState state)
    {
        State = state;
    }

    public void SetColliderEnabled(bool enabled)
    {
        if (itemCollider != null)
            itemCollider.enabled = enabled;
    }

    public InteractionInfo GetInteractionInfo()
    {
        if (pickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (pickupController.HasHeldItem && pickupController.CurrentHeldItem != this)
            return new InteractionInfo(false, "", InteractionType.Press);

        switch (State)
        {
            case ItemState.InWorld:
            case ItemState.InSocket:
                return new InteractionInfo(true, "поднять", InteractionType.Press);

            case ItemState.Inspecting:
                return new InteractionInfo(true, "взять", InteractionType.Press);

            default:
                return new InteractionInfo(false, "", InteractionType.Press);
        }
    }

    public void BeginInteract()
    {
        if (pickupController == null)
            return;

        pickupController.TryInteractWithItem(this);
    }

    public void UpdateInteract(float holdTime) { }
    public void EndInteract() { }
}