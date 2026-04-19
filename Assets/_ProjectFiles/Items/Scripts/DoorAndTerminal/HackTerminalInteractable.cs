using System.Collections.Generic;
using UnityEngine;

public class HackTerminalInteractable : BaseInteractable
{
    [SerializeField] private AutoVerticalDoor linkedDoor;
    [SerializeField] private ItemPickupController itemPickupController;
    [SerializeField] private HackConfig hackConfig;

    [Header("Hack Requirements")]
    [SerializeField] private ItemKind requiredItemKind = ItemKind.Decoder;

    private bool isSolved = false;
    private List<HackSignal> parsedSequence = new List<HackSignal>();

    public IReadOnlyList<HackSignal> RequiredSequence => parsedSequence;
    public int MaxMistakes => hackConfig.maxMistakes;

    private void Awake()
    {
        interactionType = InteractionType.Press;
        itemPickupController ??= FindFirstObjectByType<ItemPickupController>();
        parsedSequence = HackCodeUtility.ParseSequence(hackConfig.encodedSequence);
    }

    public override InteractionInfo GetInteractionInfo()
    {
        if (isSolved)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (itemPickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (!itemPickupController.IsHoldingItemKind(requiredItemKind))
            return new InteractionInfo(true, "нужен дешифратор", InteractionType.Press);

        return new InteractionInfo(true, "взломать", InteractionType.Press);
    }

    protected override bool CanInteract()
    {
        return !isSolved;
    }

    public override void BeginInteract()
    {
        // Пусто.
        // Реальный вход в hack mode делает InteractionController,
        // когда видит этот объект и проверяет CanStartHack().
    }

    public bool CanStartHack()
    {
        if (isSolved)
            return false;

        if (itemPickupController == null)
            return false;

        return itemPickupController.IsHoldingItemKind(requiredItemKind);
    }

    public void OnHackSuccess()
    {
        isSolved = true;

        if (linkedDoor != null)
            linkedDoor.OpenDoor();
    }

    public string GetSequenceProgressText(int completedSteps)
    {
        return HackCodeUtility.ProgressString(parsedSequence, completedSteps);
    }

    public string GetEncodedSequence()
    {
        return hackConfig.encodedSequence;
    }

    public string GetDisplaySequence()
    {
        return HackCodeUtility.ToSymbolString(hackConfig.encodedSequence, true);
    }
}