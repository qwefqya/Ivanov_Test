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

        // Если предмет лежал в сокете — очищаем сокет
        if (item.HomeSocket != null && item.HomeSocket.CurrentItem == item)
        {
            item.HomeSocket.ClearItem();
        }

        // Во время осмотра коллайдер выключаем, чтобы он не мешал raycast
        item.SetColliderEnabled(false);

        AttachToPoint(item.transform, inspectAnchor);

        inspectView.Show(item.Definition.description);
        SetPlayerControl(false);
    }

    private void ConfirmPickup(ItemInteractable item)
    {
        CurrentHeldItem = item;
        CurrentInspectingItem = null;

        item.SetState(ItemState.Held);

        // В руке коллайдер тоже выключен, чтобы не перекрывать сокеты
        item.SetColliderEnabled(false);

        AttachToPoint(item.transform, heldItemAnchor);

        inspectView.Hide();
        SetPlayerControl(true);
    }

    public bool CanPlaceIntoSocket(ItemSocket socket)
    {
        if (CurrentHeldItem == null)
            return false;

        if (socket == null)
            return false;

        return true;
    }

    public void TryPlaceHeldItemIntoSocket(ItemSocket socket)
    {
        if (!CanPlaceIntoSocket(socket))
            return;

        PlaceHeldItem(socket);
    }

    private void PlaceHeldItem(ItemSocket socket)
    {
        ItemInteractable item = CurrentHeldItem;

        CurrentHeldItem = null;

        item.SetState(ItemState.InSocket);
        socket.SetItem(item);

        // Когда предмет снова в мире/сокете — включаем коллайдер обратно
        item.SetColliderEnabled(true);

        AttachToPoint(item.transform, socket.ItemPoint);
    }

    private void AttachToPoint(Transform itemTransform, Transform targetPoint)
    {
        itemTransform.SetParent(targetPoint, false);
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