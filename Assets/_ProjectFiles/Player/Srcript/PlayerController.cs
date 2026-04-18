using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1.0f;

    private CharacterController characterController;
    private InputAction moveAction;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        moveAction = new InputAction("Move", InputActionType.Value);

        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        moveAction.Dispose();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
