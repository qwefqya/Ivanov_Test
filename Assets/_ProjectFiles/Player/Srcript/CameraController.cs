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
      //  lookAction.Dispose();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>() * config.sensitivity;

        // горизонтальный поворот (влево/вправо)
        transform.parent.Rotate(Vector3.up * mouseDelta.x);

        // вертикальный поворот (вверх/вниз)
        rotationX -= mouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -config.maxAngle, config.maxAngle);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}