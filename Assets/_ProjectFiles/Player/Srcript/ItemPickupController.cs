using UnityEngine;

public class ItemPickupController : MonoBehaviour
{
    [Header("Anchors")]
    [SerializeField] private Transform inspectAnchor;
    [SerializeField] private Transform heldItemAnchor;

    [Header("UI")]
    [SerializeField] private ItemInspectView inspectView;

    [Header("Player Control")]
    [SerializeField] private PlayerController playerMovement;
    [SerializeField] private CameraController playerLook;

    public ItemInteractable CurrentInspectingItem { get; private set; }
    public ItemInteractable CurrentHeldItem { get; private set; }

    public bool HasHeldItem => CurrentHeldItem != null;
    public bool IsInspecting => CurrentInspectingItem != null;

    public void TryInteractWithItem(ItemInteractable item)
    {
        if (item == null)
            return;

        if (CurrentHeldItem != null && CurrentHeldItem != item)
            return;

        if (CurrentInspectingItem == null && CurrentHeldItem == null)
        {
            BeginInspect(item);
            return;
        }

        if (CurrentInspectingItem == item)
        {
            ConfirmPickup(item);
        }
    }

    private void BeginInspect(ItemInteractable item)
    {
        CurrentInspectingItem = item;
        item.SetState(ItemState.Inspecting);

        AttachToPoint(item.transform, inspectAnchor);

        SetPlayerControl(false);
        inspectView.Show(item.Definition.description);
    }

    private void ConfirmPickup(ItemInteractable item)
    {
        CurrentHeldItem = item;
        CurrentInspectingItem = null;

        item.SetState(ItemState.Held);

        AttachToPoint(item.transform, heldItemAnchor);

        inspectView.Hide();
        SetPlayerControl(true);
    }

    private void AttachToPoint(Transform itemTransform, Transform targetPoint)
    {
        itemTransform.SetParent(targetPoint);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;
    }

    private void SetPlayerControl(bool enabled)
    {
        if (playerMovement != null)
            playerMovement.enabled = enabled;

        if (playerLook != null)
            playerLook.enabled = enabled;
    }
}