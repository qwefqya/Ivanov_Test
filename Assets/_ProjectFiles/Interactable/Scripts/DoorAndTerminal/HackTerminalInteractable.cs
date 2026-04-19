using System.Collections.Generic;
using UnityEngine;

public class HackTerminalInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private AutoVerticalDoor linkedDoor;
    [SerializeField] private ItemPickupController itemPickupController;

    [Header("Hack Requirements")]
    [SerializeField] private ItemKind requiredItemKind = ItemKind.Decoder;

    [Header("Sequence")]
    [SerializeField] private string encodedSequence = "1001";

    [Header("Failure")]
    [SerializeField] private int maxMistakes = 3;

    private bool isSolved = false;
    private List<HackSignal> parsedSequence = new List<HackSignal>();

    public IReadOnlyList<HackSignal> RequiredSequence => parsedSequence;
    public int MaxMistakes => maxMistakes;

    

    private void Awake()
    {
        if (itemPickupController == null)
            itemPickupController = FindFirstObjectByType<ItemPickupController>();

        parsedSequence = HackCodeUtility.ParseSequence(encodedSequence);
    }

    public InteractionInfo GetInteractionInfo()
    {
        if (isSolved)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (itemPickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (!itemPickupController.IsHoldingItemKind(requiredItemKind))
            return new InteractionInfo(true, "нужен дешифратор", InteractionType.Press);

        return new InteractionInfo(true, "взломать", InteractionType.Press);
    }

    public void BeginInteract()
    {
        // Запуск режима теперь делает InteractionController
    }

    public void UpdateInteract(float holdTime) { }
    public void EndInteract() { }

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
        return encodedSequence;
    }

    public string GetDisplaySequence()
    {
        return HackCodeUtility.ToSymbolString(encodedSequence, true);
    }
}