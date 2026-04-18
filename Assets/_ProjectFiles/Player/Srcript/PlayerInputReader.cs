using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;

    public bool InteractStartedThisFrame { get; private set; }
    public bool InteractReleasedThisFrame { get; private set; }
    public bool IsInteractHeld { get; private set; }

    private void OnEnable()
    {
        interactAction.action.started += OnInteractStarted;
        interactAction.action.canceled += OnInteractCanceled;
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.started -= OnInteractStarted;
        interactAction.action.canceled -= OnInteractCanceled;
        interactAction.action.Disable();
    }

    private void LateUpdate()
    {
        InteractStartedThisFrame = false;
        InteractReleasedThisFrame = false;
    }

    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        IsInteractHeld = true;
        InteractStartedThisFrame = true;
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        IsInteractHeld = false;
        InteractReleasedThisFrame = true;
    }
}