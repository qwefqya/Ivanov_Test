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

    //подписываемся на action 



    private void OnDisable()
    {
        interactAction.action.started -= OnInteractStarted;
        interactAction.action.canceled -= OnInteractCanceled;
        interactAction.action.Disable();
    }

    //отписка от action

    private void LateUpdate()
    {
        InteractStartedThisFrame = false;
        InteractReleasedThisFrame = false;
    }

    // делает флаги одноразовыми для каждого кадра

    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        IsInteractHeld = true;
        InteractStartedThisFrame = true;
    }

    //ставим кнопку нажатой в кадре

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        IsInteractHeld = false;
        InteractReleasedThisFrame = true;
    }

    //ставим кнопку не нажатой в кадре
}