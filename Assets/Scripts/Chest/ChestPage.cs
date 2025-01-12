using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Inventory.Model;
using Inventory.UI;  
using TMPro;


public class ChestPage : MonoBehaviour
{
    [Header("Chest UI Setup")]
    [SerializeField]
    private UIInventoryItem itemPrefab;      // The same prefab used in InventoryPage
    [SerializeField]
    private RectTransform contentPanel;      // The parent container for item slots
    [SerializeField]
    private TMP_Text chestHeader;           

    private List<UIInventoryItem> uiItems = new List<UIInventoryItem>();
    private Chest currentChest;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowChest(Chest chest)
    {
        currentChest = chest;

        if (chestHeader != null)
            chestHeader.text = "Objets";

        // We first clear old items if any
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        uiItems.Clear();

        // Now we rebuild the slot UI for each item in the chest
        for (int i = 0; i < chest.chestItems.Count; i++)
        {
            var invItem = chest.chestItems[i];

            var uiSlot = Instantiate(itemPrefab, contentPanel);
            uiSlot.transform.localScale = Vector3.one; 

            if (invItem.IsEmpty)
            {
                uiSlot.ResetData();
            }
            else
            {
                // Display the item’s image and quantity
                uiSlot.SetData(invItem.item.ItemImage, invItem.quantity);
            }

            uiSlot.OnItemClicked += (UIInventoryItem clickedItem) => // lorsque l'on clique, on récupère l'item associé au slot
            {
                int slotIndex = uiItems.IndexOf(clickedItem);
                currentChest.TakeItem(slotIndex);
            };
            uiSlot.OnRightMouseBtnClick += (UIInventoryItem clickedItem) =>
            {
                int slotIndex = uiItems.IndexOf(clickedItem);
                currentChest.TakeItem(slotIndex);
            };


            uiItems.Add(uiSlot);
        }

        gameObject.SetActive(true);
    }

    public void RefreshChest(Chest chest)
    {
        ShowChest(chest);
    }

    public void HideChest()
    {
        // Hide the UI
        gameObject.SetActive(false);
        currentChest = null;
    }
}
