using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogUI : MonoBehaviour
{
    public static QuestLogUI Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject questPanel;  // Parent Panel for Quest Log
    [SerializeField] private GameObject questEntryPrefab; // Prefab for each quest entry
    [SerializeField] private Transform questListContainer; // Parent for quest entries

    private List<GameObject> questEntries = new List<GameObject>(); // Track UI entries
    private List<string> questTitles = new List<string>();
    private List<string> questDescriptions = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddQuest(Quest quest, string npcName)
    {
        // Format quest details (FIX: Include required quantities)
        string questTitle = $"Mission : {quest.questName}";

        List<string> requiredItemsText = new List<string>();
        for (int i = 0; i < quest.requiredItems.Count; i++)
        {
            string itemName = quest.requiredItems[i].Name;
            int quantity = (i < quest.requiredQuantities.Count) ? quest.requiredQuantities[i] : 1; // Default to 1 if missing
            requiredItemsText.Add($"{itemName} x{quantity}");
        }

        string questDescription = $"Apporte {string.Join(", ", requiredItemsText)} à {npcName}.";

        // Store quest data in lists
        questTitles.Add(questTitle);
        questDescriptions.Add(questDescription);

        // Create a new quest UI entry
        GameObject newEntry = Instantiate(questEntryPrefab, questListContainer);
        TMP_Text[] textComponents = newEntry.GetComponentsInChildren<TMP_Text>();

        textComponents[0].text = questTitle;      // Quest Title
        textComponents[1].text = questDescription; // Quest Description (FIXED: Now includes required quantities)

        questEntries.Add(newEntry); // Track UI elements

        // Make sure quest panel is visible
        questPanel.SetActive(true);
    }

    public void RemoveQuest(Quest quest)
    {
        int index = questTitles.FindIndex(q => q.Contains(quest.questName));
        if (index != -1)
        {
            // Remove from lists
            questTitles.RemoveAt(index);
            questDescriptions.RemoveAt(index);

            // Destroy corresponding UI element
            Destroy(questEntries[index]);
            questEntries.RemoveAt(index);
        }

        // Hide panel if no more quests
        if (questEntries.Count == 0)
        {
            questPanel.SetActive(false);
        }
    }
}
