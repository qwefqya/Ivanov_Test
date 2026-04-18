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

    public bool IsHoldingItemKind(ItemKind kind)
    {
        if (CurrentHeldItem == null)
            return false;

        if (CurrentHeldItem.Definition == null)
            return false;

        return CurrentHeldItem.Definition.itemKind == kind;
    }

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
        if (item.CurrentSocket != null && item.CurrentSocket.CurrentItem == item)
        {
            item.CurrentSocket.ClearItem();
        }

        // Во время осмотра коллайдер выключаем, чтобы он не мешал raycast
        item.SetColliderEnabled(false);

        

        AttachToPoint(item.transform, inspectAnchor);

        if (item.TryGetComponent(out NotePresentation note))
        {
            note.PlayOpen();
        }

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

        if (socket.CurrentItem != null)
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

        if (item.TryGetComponent(out NotePresentation note))
    {
        note.ForceClosedState();
    }

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

 

    public bool ConsumeHeldItemIfMatches(ItemKind kind)
    {
        if (!IsHoldingItemKind(kind))
            return false;

        ItemInteractable item = CurrentHeldItem;

        CurrentHeldItem = null;
        item.SetState(ItemState.Consumed);

        if (item.TryGetComponent(out NotePresentation note))
        {
            note.ForceClosedState();
        }

        Destroy(item.gameObject);
        return true;
    }
}