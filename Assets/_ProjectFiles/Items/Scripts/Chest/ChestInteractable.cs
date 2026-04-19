using UnityEngine;

public class ChestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string openTrigger = "Open";

    private ItemPickupController itemPickupController;
    private bool isOpened;

    private void Awake()
    {
        itemPickupController = FindFirstObjectByType<ItemPickupController>();
    }

    public InteractionInfo GetInteractionInfo()
    {
        if (isOpened)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (itemPickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (itemPickupController.IsHoldingItemKind(ItemKind.Key))
            return new InteractionInfo(true, "открыть", InteractionType.Press);

        return new InteractionInfo(true, "нужен ключ", InteractionType.Press);
    }

    public void BeginInteract()
    {
        if (isOpened)
            return;

        if (itemPickupController == null)
            return;

        bool keyConsumed = itemPickupController.ConsumeHeldItemIfMatches(ItemKind.Key);

        if (!keyConsumed)
            return;

        OpenChest();
    }

    public void UpdateInteract(float holdTime)
    {
    }

    public void EndInteract()
    {
    }

    private void OpenChest()
    {
        isOpened = true;
       // Debug.Log(isOpened);

        if (animator != null)
            animator.SetTrigger(openTrigger);
    }
}