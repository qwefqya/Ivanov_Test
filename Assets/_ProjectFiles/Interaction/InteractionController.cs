using System.Collections.Generic;
using UnityEngine;

public enum HackSignal
{
    Short,
    Long
}

public class InteractionController : MonoBehaviour
{
    private enum InteractionMode
    {
        World,
        InspectItem,
        HackSequence
    }

    [Header("World Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayer;

    [Header("References")]
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private InteractionPromptView promptView;
    [SerializeField] private ItemPickupController itemPickupController;
    [SerializeField] private PlayerController playerMovement;
    [SerializeField] private CameraController playerLook;

    [Header("Hack Settings")]
    [SerializeField] private float longPressThreshold = 0.5f;

    private IInteractable currentInteractable;
    private IInteractable activeInteractable;

    private bool isInteracting;
    private float holdTime;

    private InteractionMode currentMode = InteractionMode.World;

    // Hack mode
    private HackTerminalInteractable activeHackTerminal;
    private float currentHackPressTime = 0f;
    private int currentHackStep = 0;
    private bool waitForHackRelease = false;
    private int currentHackMistakes = 0;

    private void Update()
    {
        switch (currentMode)
        {
            case InteractionMode.World:
                UpdateWorldMode();
                break;

            case InteractionMode.InspectItem:
                UpdateInspectMode();
                break;

            case InteractionMode.HackSequence:
                UpdateHackMode();
                break;
        }
    }

    private void UpdateWorldMode()
    {
        UpdateCurrentInteractable();
        ProcessWorldInteraction();
        UpdateWorldPrompt();

        if (itemPickupController != null && itemPickupController.IsInspecting)
        {
            currentMode = InteractionMode.InspectItem;
        }
    }

    private void UpdateInspectMode()
    {
        UpdateInspectPrompt();

        if (inputReader != null && inputReader.InteractStartedThisFrame)
        {
            ItemInteractable inspectingItem = itemPickupController != null
                ? itemPickupController.CurrentInspectingItem
                : null;

            if (inspectingItem != null)
                inspectingItem.BeginInteract();
        }

        if (itemPickupController == null || !itemPickupController.IsInspecting)
        {
            currentMode = InteractionMode.World;
        }
    }

    private void UpdateHackMode()
    {
        if (activeHackTerminal == null || inputReader == null)
        {
            ExitHackMode();
            return;
        }

        UpdateHackPrompt();

        if (waitForHackRelease)
        {
            if (inputReader.InteractReleasedThisFrame)
            {
                waitForHackRelease = false;
                currentHackPressTime = 0f;
            }

            return;
        }

        if (inputReader.IsInteractHeld)
        {
            currentHackPressTime += Time.deltaTime;
        }

        if (inputReader.InteractReleasedThisFrame)
        {
            HackSignal signal = currentHackPressTime >= longPressThreshold
                ? HackSignal.Long
                : HackSignal.Short;

            currentHackPressTime = 0f;
            SubmitHackSignal(signal);
        }
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

    private void ProcessWorldInteraction()
    {
        if (inputReader == null)
            return;

        if (inputReader.InteractStartedThisFrame && currentInteractable != null)
        {
            if (currentInteractable is HackTerminalInteractable hackTerminal)
            {
                InteractionInfo info = hackTerminal.GetInteractionInfo();

                if (info.IsAvailable && hackTerminal.CanStartHack())
                {
                    EnterHackMode(hackTerminal);
                    return;
                }
            }

            activeInteractable = currentInteractable;
            isInteracting = true;
            holdTime = 0f;

            activeInteractable.BeginInteract();

            InteractionInfo interactionInfo = activeInteractable.GetInteractionInfo();

            if (interactionInfo.InteractionType == InteractionType.Press)
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

    private void UpdateWorldPrompt()
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
        promptView.Show($"{prefix} Ч {info.PromptText}");
    }

    private void UpdateInspectPrompt()
    {
        if (itemPickupController == null || itemPickupController.CurrentInspectingItem == null)
        {
            promptView.Hide();
            return;
        }

        InteractionInfo info = itemPickupController.CurrentInspectingItem.GetInteractionInfo();

        if (!info.IsAvailable)
        {
            promptView.Hide();
            return;
        }

        promptView.Show($"E Ч {info.PromptText}");
    }

    private void EnterHackMode(HackTerminalInteractable terminal)
    {
        activeHackTerminal = terminal;
        currentHackStep = 0;
        currentHackPressTime = 0f;
        currentHackMistakes = 0;
        waitForHackRelease = true;

        currentMode = InteractionMode.HackSequence;

        SetPlayerControl(false);
        UpdateHackPrompt();
    }

    private void ExitHackMode()
    {
        activeHackTerminal = null;
        currentHackStep = 0;
        currentHackPressTime = 0f;
        currentHackMistakes = 0;
        waitForHackRelease = false;

        currentMode = InteractionMode.World;

        SetPlayerControl(true);
        promptView.Hide();
    }

    private void SubmitHackSignal(HackSignal signal)
    {
        if (activeHackTerminal == null)
        {
            ExitHackMode();
            return;
        }

        IReadOnlyList<HackSignal> sequence = activeHackTerminal.RequiredSequence;

        if (sequence == null || sequence.Count == 0)
        {
            ExitHackMode();
            return;
        }

        if (signal != sequence[currentHackStep])
        {
            currentHackMistakes++;
            currentHackStep = 0;

            if (currentHackMistakes >= activeHackTerminal.MaxMistakes)
            {
                ExitHackMode();
                return;
            }

            UpdateHackPrompt();
            return;
        }

        currentHackStep++;

        if (currentHackStep >= sequence.Count)
        {
            activeHackTerminal.OnHackSuccess();
            ExitHackMode();
        }
        else
        {
            UpdateHackPrompt();
        }
    }

    private void UpdateHackPrompt()
    {
        if (activeHackTerminal == null)
        {
            promptView.Hide();
            return;
        }

        promptView.Show(
            $"E Ч ввод кода [{activeHackTerminal.GetSequenceProgressText(currentHackStep)}] ќшибки: {currentHackMistakes}/{activeHackTerminal.MaxMistakes}"
        );
    }

    private void ResetInteraction()
    {
        isInteracting = false;
        activeInteractable = null;
        holdTime = 0f;
    }

    private void SetPlayerControl(bool enabled)
    {
        if (playerMovement != null)
            playerMovement.enabled = enabled;

        if (playerLook != null)
            playerLook.enabled = enabled;
    }
}