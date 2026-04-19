using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "NPC";
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private bool isQuestNpc = true;

    private DialogueController dialogueController;
    private QuestController questController;

    private void Awake()
    {
        dialogueController = FindFirstObjectByType<DialogueController>();
        questController = FindFirstObjectByType<QuestController>();
    }

    public InteractionInfo GetInteractionInfo()
    {
        if (dialogueController == null)
            return new InteractionInfo(false, "", InteractionType.Press);

        if (dialogueController.IsDialogueActive)
            return new InteractionInfo(false, "", InteractionType.Press);

        return new InteractionInfo(true, "разговор", InteractionType.Press);
    }

    public void BeginInteract()
    {
        if (dialogueController == null || dialogueData == null)
            return;

        // Если уже есть активный квест и он ещё не выполнен — пробуем сдать предмет
        if (isQuestNpc && questController != null && questController.HasActiveQuest && !questController.IsQuestCompleted)
        {
            questController.TryCompleteQuest();
        }

        dialogueController.StartDialogue(this, dialogueData, isQuestNpc);
    }

    public void UpdateInteract(float holdTime) { }
    public void EndInteract() { }

    public string NpcName => npcName;
}