using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInspectRotator : MonoBehaviour
{
    [SerializeField] private ItemPickupController itemPickupController;
    [SerializeField] private float rotationSpeed = 0.2f;

    private void Update()
    {
        if (itemPickupController == null)
            return;

        if (!itemPickupController.IsInspecting)
            return;

        if (itemPickupController.CurrentInspectingItem == null)
            return;

        if (!Mouse.current.leftButton.isPressed)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        Transform itemTransform = itemPickupController.CurrentInspectingItem.transform;

        itemTransform.Rotate(Vector3.up, -mouseDelta.x * rotationSpeed, Space.World);
        itemTransform.Rotate(Vector3.right, mouseDelta.y * rotationSpeed, Space.World);
    }
}