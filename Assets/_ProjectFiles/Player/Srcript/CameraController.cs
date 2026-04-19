using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerConfig config;

    private float rotationX = 0f;
    private InputAction lookAction;

    private void OnEnable()
    {
        lookAction = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotationX = transform.localEulerAngles.x;

        // Переводим угол из диапазона 0..360 в -180..180
        if (rotationX > 180f)
            rotationX -= 360f;
    }

    private void Update()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>() * config.sensitivity;
        transform.parent.Rotate(Vector3.up * mouseDelta.x);
        rotationX -= mouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -config.maxAngle, config.maxAngle);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}