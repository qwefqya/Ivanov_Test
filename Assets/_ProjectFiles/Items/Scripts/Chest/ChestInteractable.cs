using UnityEngine;

public class ChestInteractable : BaseInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string openTrigger = "Open";
    [SerializeField] private ItemKind requiredItemKind = ItemKind.Key;

    private ItemPickupController itemPickupController;
    private bool isOpened;

    private void Awake()
    {
        interactionType = InteractionType.Press;
        itemPickupController = FindFirstObjectByType<ItemPickupController>();
    }

    public override InteractionInfo GetInteractionInfo()
    {
        if (isOpened)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (itemPickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (itemPickupController.IsHoldingItemKind(requiredItemKind))
            return new InteractionInfo(true, "открыть", InteractionType.Press);

        return new InteractionInfo(true, "нужен ключ", InteractionType.Press);
    }

    public override void BeginInteract()
    {
        if (isOpened || itemPickupController == null)
            return;

        bool consumed = itemPickupController.ConsumeHeldItemIfMatches(requiredItemKind);

        if (!consumed)
            return;

        OpenChest();
    }

    private void OpenChest()
    {
        isOpened = true;

        if (animator != null)
            animator.SetTrigger(openTrigger);
    }
}