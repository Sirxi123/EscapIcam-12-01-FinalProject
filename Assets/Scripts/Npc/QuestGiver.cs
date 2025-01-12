using System;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;
using Inventory.UI;
using Inventory;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public Quest questData;
    private bool questGiven = false;
    private bool questCompleted = false;

    public GameObject dialogBox;
    public string npcName;
    public TMP_Text npcNameText;
    public TMP_Text questTitleText;
    public TMP_Text dialogText;
    public GameObject InteractionText;
    public bool playerInRange;
    public KeyCode interactKey = KeyCode.E;

    private InventoryController playerInventory;

    private void Start()
    {
        npcNameText.text = string.IsNullOrEmpty(npcName) ? gameObject.name : npcName;
        dialogBox.SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInventory = player.GetComponent<InventoryController>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && playerInRange)
        {
            npcNameText.text = npcName;
            if (!questGiven)
            {
                GiveQuest();
            }
            else if (!questCompleted && CheckQuestCompletion())
            {
                CompleteQuest();
            }
            else if (!questCompleted)
            {
                dialogBox.SetActive(true);
                dialogText.text = "Tu ne m'as pas ramené :";
                
                foreach (var item in questData.requiredItems)       // demande les items et les liste
                {
                    dialogText.text +=  item.Name + ", ";
                }
            }
        }
    }

    private void GiveQuest()
    {
        questGiven = true;
        dialogBox.SetActive(true);
        questTitleText.text = "Mission : " + questData.questName;
        dialogText.text = questData.questDescription + "\nApporte-moi : ";
        foreach (var item in questData.requiredItems)
        {
            dialogText.text += item.Name + " (x" + questData.requiredQuantities[questData.requiredItems.IndexOf(item)] + ")" + " |"; // Ecris le texte pour le dialogue
        }

        QuestLogUI.Instance.AddQuest(questData, npcName);
    }

    private bool CheckQuestCompletion()
    {
        if (playerInventory != null)
        {
            Dictionary<int, InventoryItem> inventory = playerInventory.inventoryData.GetCurrentInventoryState();
            foreach (var requiredItem in questData.requiredItems)
            {
                bool hasItem = false;
                foreach (var item in inventory)
                {
                    if (item.Value.item == requiredItem && item.Value.quantity >= questData.requiredQuantities[questData.requiredItems.IndexOf(requiredItem)]) // Cherche l'item que la quête requiert et son nombre et valide ou non la quete
                    {
                        hasItem = true;
                        break;
                    }
                }
                if (!hasItem) return false;
            }
            return true;
        }
        return false;
    }

    private void CompleteQuest()
    {
        questCompleted = true;
        foreach (var requiredItem in questData.requiredItems)
        {
            foreach (var item in playerInventory.inventoryData.GetCurrentInventoryState())
            {
                if (item.Value.item == requiredItem)
                {
                    playerInventory.inventoryData.RemoveItem(item.Key, questData.requiredQuantities[questData.requiredItems.IndexOf(requiredItem)]);
                    break;
                }
            }
        }
        GiveRewards();
        dialogText.text = "Merci ! Voici ta récompense.";
        dialogBox.SetActive(true);
        QuestLogUI.Instance.RemoveQuest(questData); // Dis au joueur qu'il a réussi et qu'il a recu une récompense + supprime la quête de l'UI
    }

    private void GiveRewards()
    {
        for (int i = 0; i < questData.rewardItems.Count; i++)
        {
            playerInventory.inventoryData.AddItem(questData.rewardItems[i], questData.rewardQuantities[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionText.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionText.SetActive(false);
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }
}
