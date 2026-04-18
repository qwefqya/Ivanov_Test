using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float maxAngle = 75f;

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
      //  lookAction.Dispose();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>() * sensitivity;

        // горизонтальный поворот (влево/вправо)
        transform.parent.Rotate(Vector3.up * mouseDelta.x);

        // вертикальный поворот (вверх/вниз)
        rotationX -= mouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -maxAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}