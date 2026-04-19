using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private GameObject continueHint;
    [SerializeField] private Button[] choiceButtons;

    [Header("References")]
    [SerializeField] private PlayerController playerMovement;
    [SerializeField] private CameraController playerLook;
    [SerializeField] private QuestController questController;
    [SerializeField] private List<ItemInteractable> sceneItems = new List<ItemInteractable>();

    [SerializeField] private PlayerInputReader inputReader;

    private DialogueData currentDialogue;
    private NPCInteractable currentNpc;
    private int currentNodeIndex;
    private bool currentNpcIsQuestNpc;

    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        HideChoices();
    }

    private void Update()
    {
        if (!IsDialogueActive || currentDialogue == null)
            return;

        DialogueNode node = currentDialogue.nodes[currentNodeIndex];

        if (node.choices.Count == 0 && inputReader != null && inputReader.InteractStartedThisFrame)
        {
            AdvanceLinearNode();
        }
    }

    public void StartDialogue(NPCInteractable npc, DialogueData dialogue, bool isQuestNpc)
    {
        if (dialogue == null || dialogue.nodes.Count == 0)
            return;

        currentNpc = npc;
        currentDialogue = dialogue;
        currentNodeIndex = dialogue.startNodeIndex;
        currentNpcIsQuestNpc = isQuestNpc;

        IsDialogueActive = true;
        SetPlayerControl(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowCurrentNode();
    }

    private void ShowCurrentNode()
    {
        if (currentDialogue == null)
            return;

        DialogueNode node = currentDialogue.nodes[currentNodeIndex];

        if (speakerText != null)
            speakerText.text = node.speakerName;

        if (bodyText != null)
            bodyText.text = node.text;

        if (node.choices.Count > 0)
        {
            ShowChoices(node.choices);
            if (continueHint != null)
                continueHint.SetActive(false);
        }
        else
        {
            HideChoices();

            if (continueHint != null)
                continueHint.SetActive(!node.isEnding);
        }
    }

    private void AdvanceLinearNode()
    {
        DialogueNode node = currentDialogue.nodes[currentNodeIndex];

        if (node.isEnding)
        {
            EndDialogue();
            return;
        }

        // Для линейного диалога без choices переходим просто на следующий индекс
        int nextIndex = currentNodeIndex + 1;

        if (nextIndex >= currentDialogue.nodes.Count)
        {
            // Финал диалога
            if (node.givesQuest && currentNpcIsQuestNpc && questController != null && !questController.HasActiveQuest)
            {
                questController.StartRandomItemQuest(sceneItems);
            }

            EndDialogue();
            return;
        }

        currentNodeIndex = nextIndex;
        ShowCurrentNode();

        DialogueNode newNode = currentDialogue.nodes[currentNodeIndex];

        if (newNode.givesQuest && currentNpcIsQuestNpc && questController != null && !questController.HasActiveQuest)
        {
            questController.StartRandomItemQuest(sceneItems);
        }
    }

    private void ShowChoices(List<DialogueChoice> choices)
    {
        HideChoices();

        for (int i = 0; i < choiceButtons.Length && i < choices.Count; i++)
        {
            int capturedIndex = i;
            choiceButtons[i].gameObject.SetActive(true);

            TextMeshProUGUI label = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
                label.text = choices[i].choiceText;

            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => SelectChoice(choices[capturedIndex]));
        }
    }

    private void HideChoices()
    {
        foreach (var button in choiceButtons)
        {
            if (button == null)
                continue;

            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

    private void SelectChoice(DialogueChoice choice)
    {
        if (currentDialogue == null)
            return;

        if (choice.nextNodeIndex < 0 || choice.nextNodeIndex >= currentDialogue.nodes.Count)
        {
            EndDialogue();
            return;
        }

        currentNodeIndex = choice.nextNodeIndex;
        ShowCurrentNode();
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
        currentDialogue = null;
        currentNpc = null;
        currentNodeIndex = 0;
        currentNpcIsQuestNpc = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        HideChoices();
        SetPlayerControl(true);
    }

    private void SetPlayerControl(bool enabled)
    {
        if (playerMovement != null)
            playerMovement.enabled = enabled;

        if (playerLook != null)
            playerLook.enabled = enabled;
    }
}