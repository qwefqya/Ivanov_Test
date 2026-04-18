using System.Collections.Generic;
using UnityEngine;

public enum HackSignal
{
    Short,
    Long
}
public class HackTerminalInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private AutoVerticalDoor linkedDoor;
    [SerializeField] private HackSequenceController hackController;
    [SerializeField] private ItemPickupController itemPickupController;

    [Header("Required Sequence")]
    [SerializeField]
    private List<HackSignal> requiredSequence = new List<HackSignal>
    {
        HackSignal.Long,
        HackSignal.Short,
        HackSignal.Short,
        HackSignal.Long
    };

    [Header("Requirements")]
    [SerializeField] private ItemKind requiredItemKind = ItemKind.Decoder;

    private bool isSolved = false;

    public IReadOnlyList<HackSignal> RequiredSequence => requiredSequence;

    public InteractionInfo GetInteractionInfo()
    {
        if (isSolved)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (hackController != null && hackController.IsHacking)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (itemPickupController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (!itemPickupController.IsHoldingItemKind(requiredItemKind))
            return new InteractionInfo(true, "нужен дешифратор", InteractionType.Press);

        return new InteractionInfo(true, "взломать", InteractionType.Press);
    }

    public void BeginInteract()
    {
        if (isSolved)
            return;

        if (hackController == null || itemPickupController == null)
            return;

        if (!itemPickupController.IsHoldingItemKind(requiredItemKind))
            return;

        hackController.StartHack(this);
    }

    public void UpdateInteract(float holdTime) { }
    public void EndInteract() { }

    public void OnHackSuccess()
    {
        isSolved = true;

        if (linkedDoor != null)
            linkedDoor.OpenDoor();
    }

    public string GetSequenceProgressText(int completedSteps)
    {
        string result = "";

        for (int i = 0; i < requiredSequence.Count; i++)
        {
            if (i < completedSteps)
            {
                result += requiredSequence[i] == HackSignal.Long ? "Ч " : "Х ";
            }
            else
            {
                result += "_ ";
            }
        }

        return result.TrimEnd();
    }
}