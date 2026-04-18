using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private InteractionPromptView promptView;
    [SerializeField] private ItemPickupController itemPickupController;

    private IInteractable currentInteractable;
    private IInteractable activeInteractable;

    private bool isInteracting;
    private float holdTime;

    private void Update()
    {
        UpdateCurrentInteractable();
        ProcessInteraction();
        UpdatePrompt();
    }

    private void UpdateCurrentInteractable()
    {
        currentInteractable = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();

            if (currentInteractable == null)
                currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
        }
    }

    private void ProcessInteraction()
    {
        IInteractable target = null;

        // Если сейчас идёт осмотр предмета, второй E должен работать
        // не через raycast, а через текущий предмет в осмотре
        if (itemPickupController != null && itemPickupController.IsInspecting)
        {
            target = itemPickupController.CurrentInspectingItem;
        }
        else
        {
            target = currentInteractable;
        }

        if (inputReader.InteractStartedThisFrame && target != null)
        {
            activeInteractable = target;
            isInteracting = true;
            holdTime = 0f;

            activeInteractable.BeginInteract();

            InteractionInfo info = activeInteractable.GetInteractionInfo();

            if (info.InteractionType == InteractionType.Press)
            {
                activeInteractable.EndInteract();
                ResetInteraction();
                return;
            }
        }

        if (isInteracting && activeInteractable != null && inputReader.IsInteractHeld)
        {
            holdTime += Time.deltaTime;
            activeInteractable.UpdateInteract(holdTime);
        }

        if (isInteracting && activeInteractable != null && inputReader.InteractReleasedThisFrame)
        {
            activeInteractable.EndInteract();
            ResetInteraction();
        }
    }

    private void UpdatePrompt()
    {
        // Во время осмотра prompt можно брать с предмета в осмотре
        if (itemPickupController != null && itemPickupController.IsInspecting && itemPickupController.CurrentInspectingItem != null)
        {
            InteractionInfo inspectInfo = itemPickupController.CurrentInspectingItem.GetInteractionInfo();

            if (inspectInfo.IsAvailable)
            {
                string inspectPrefix = inspectInfo.InteractionType == InteractionType.Hold ? "Hold E" : "E";
                promptView.Show($"{inspectPrefix} — {inspectInfo.PromptText}");
                return;
            }
        }

        if (currentInteractable == null)
        {
            promptView.Hide();
            return;
        }

        InteractionInfo info = currentInteractable.GetInteractionInfo();

        if (!info.IsAvailable)
        {
            promptView.Hide();
            return;
        }

        string prefix = info.InteractionType == InteractionType.Hold ? "Hold E" : "E";
        promptView.Show($"{prefix} — {info.PromptText}");
    }

    private void ResetInteraction()
    {
        isInteracting = false;
        activeInteractable = null;
        holdTime = 0f;
    }
}