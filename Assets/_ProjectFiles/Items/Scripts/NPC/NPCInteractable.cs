using UnityEngine;

public class NPCInteractable : BaseInteractable
{
    [SerializeField] private string npcName = "NPC";
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private bool isQuestNpc = true;

    private DialogueController dialogueController;
    private QuestController questController;

    private void Awake()
    {
        promptText = "разговор";
        interactionType = InteractionType.Press;

        dialogueController = FindFirstObjectByType<DialogueController>();
        questController = FindFirstObjectByType<QuestController>();
    }

    protected override bool CanInteract()
    {
        return dialogueController != null && !dialogueController.IsDialogueActive;
    }

    public override void BeginInteract()
    {
        if (dialogueController == null || dialogueData == null)
            return;

        if (isQuestNpc && questController != null && questController.HasActiveQuest && !questController.IsQuestCompleted)
        {
            questController.TryCompleteQuest();
        }

        dialogueController.StartDialogue(this, dialogueData, isQuestNpc);
    }

    public string NpcName => npcName;
}