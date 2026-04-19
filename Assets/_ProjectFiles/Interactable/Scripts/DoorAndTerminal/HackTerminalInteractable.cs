using System.Collections.Generic;
using UnityEngine;

public class HackTerminalInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private AutoVerticalDoor linkedDoor;
    [SerializeField] private ItemPickupController itemPickupController;

    [Header("Hack Requirements")]
    [SerializeField] private ItemKind requiredItemKind = ItemKind.Decoder;

    [Header("Required Sequence")]
    [SerializeField]
    private List<HackSignal> requiredSequence = new List<HackSignal>
    {
        HackSignal.Long,
        HackSignal.Short,
        HackSignal.Short,
        HackSignal.Long
    };

    private bool isSolved = false;

    public IReadOnlyList<HackSignal> RequiredSequence => requiredSequence;

    private void Awake()
    {
        if (itemPickupController == null)
            itemPickupController = FindFirstObjectByType<ItemPickupController>();
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
        // Ничего не делаем здесь.
        // Запуск режима взлома теперь контролирует InteractionController.
    }

    public void UpdateInteract(float holdTime)
    {
    }

    public void EndInteract()
    {
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
        string result = "";

        for (int i = 0; i < requiredSequence.Count; i++)
        {
            if (i < completedSteps)
            {
                result += requiredSequence[i] == HackSignal.Long ? "— " : "• ";
            }
            else
            {
                result += "_ ";
            }
        }

        return result.TrimEnd();
    }
}