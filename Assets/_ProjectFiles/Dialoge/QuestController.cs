using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestController : MonoBehaviour
{
    [SerializeField] private ItemPickupController itemPickupController;

    [Header("UI")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private GameObject checkmarkObject;

    private ItemDefinition requiredItem;
    private bool questActive = false;
    private bool questCompleted = false;

    private void Awake()
    {
        HideQuestUI();
    }

    public bool HasActiveQuest => questActive;
    public bool IsQuestCompleted => questCompleted;
    public ItemDefinition RequiredItem => requiredItem;

    public void StartRandomItemQuest(List<ItemInteractable> sceneItems)
    {
        List<ItemInteractable> candidates = new List<ItemInteractable>();

        foreach (var item in sceneItems)
        {
            if (item == null || item.Definition == null)
                continue;
                       
            candidates.Add(item);
        }

        if (candidates.Count == 0)
            return;

        int index = Random.Range(0, candidates.Count);
        requiredItem = candidates[index].Definition;

        questActive = true;
        questCompleted = false;

        ShowQuestUI();
    }

    public bool TryCompleteQuest()
    {
        if (!questActive || questCompleted || requiredItem == null || itemPickupController == null)
            return false;

        ItemInteractable heldItem = itemPickupController.GetHeldItem();

        if (heldItem == null || heldItem.Definition == null)
            return false;

        if (heldItem.Definition != requiredItem)
            return false;

        itemPickupController.ConsumeHeldItem();
        questCompleted = true;

        UpdateQuestUI();
        return true;
    }

    private void ShowQuestUI()
    {
        if (questPanel != null)
            questPanel.SetActive(true);

        UpdateQuestUI();
    }

    private void UpdateQuestUI()
    {
        if (questText != null)
        {
            string itemName = requiredItem != null ? requiredItem.displayName : "предмет";
            questText.text = $"[ ] Принести: {itemName}";
        }

        if (checkmarkObject != null)
            checkmarkObject.SetActive(questCompleted);

        if (questCompleted && questText != null)
        {
            string itemName = requiredItem != null ? requiredItem.displayName : "предмет";
            questText.text = $"[x] Принести: {itemName}";
        }
    }

    private void HideQuestUI()
    {
        if (questPanel != null)
            questPanel.SetActive(false);

        if (checkmarkObject != null)
            checkmarkObject.SetActive(false);
    }
}