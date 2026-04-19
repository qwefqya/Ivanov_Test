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
    [SerializeField] private Button[] choiceButtons;

    [Header("References")]
    [SerializeField] private PlayerController playerMovement;
    [SerializeField] private CameraController playerLook;
    [SerializeField] private QuestController questController;
    [SerializeField] private List<ItemInteractable> sceneItems = new List<ItemInteractable>();

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

    public void StartDialogue(NPCInteractable npc, DialogueData dialogue, bool isQuestNpc)
    {
        if (dialogue == null || dialogue.nodes == null || dialogue.nodes.Count == 0)
            return;

        currentNpc = npc;
        currentDialogue = dialogue;
        currentNodeIndex = dialogue.startNodeIndex;
        currentNpcIsQuestNpc = isQuestNpc;

        IsDialogueActive = true;

        SetPlayerControl(false);
        SetCursorState(true);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowCurrentNode();
    }

    private void ShowCurrentNode()
    {
        if (currentDialogue == null)
            return;

        if (currentNodeIndex < 0 || currentNodeIndex >= currentDialogue.nodes.Count)
        {
            EndDialogue();
            return;
        }

        DialogueNode node = currentDialogue.nodes[currentNodeIndex];

        // Выдача квеста срабатывает при показе этого узла один раз
        if (node.givesQuest && currentNpcIsQuestNpc && questController != null && !questController.HasActiveQuest)
        {
            questController.StartRandomItemQuest(sceneItems);
        }

        if (speakerText != null)
            speakerText.text = node.speakerName;

        if (bodyText != null)
            bodyText.text = node.text;

        if (node.choices != null && node.choices.Count > 0)
        {
            ShowChoices(node.choices);
        }
        else
        {
            HideChoices();
        }
    }

    private void ShowChoices(List<DialogueChoice> choices)
    {
        HideChoices();

        for (int i = 0; i < choiceButtons.Length && i < choices.Count; i++)
        {
            int capturedIndex = i;

            if (choiceButtons[i] == null)
                continue;

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
        if (choiceButtons == null)
            return;

        foreach (Button button in choiceButtons)
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

        // Любой отрицательный индекс = закончить диалог
        if (choice.nextNodeIndex < 0)
        {
            EndDialogue();
            return;
        }

        if (choice.nextNodeIndex >= currentDialogue.nodes.Count)
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
        SetCursorState(false);
    }

    private void SetPlayerControl(bool enabled)
    {
        if (playerMovement != null)
            playerMovement.enabled = enabled;

        if (playerLook != null)
            playerLook.enabled = enabled;
    }

    private void SetCursorState(bool dialogueActive)
    {
        if (dialogueActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}