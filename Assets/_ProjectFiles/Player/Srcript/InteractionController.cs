using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private InteractionPromptView promptView;

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
        if (inputReader.InteractStartedThisFrame && currentInteractable != null)
        {
            activeInteractable = currentInteractable;
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