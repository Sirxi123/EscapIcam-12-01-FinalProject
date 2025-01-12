using UnityEngine;
using System.Collections.Generic;
using Inventory.Model;  

public class Chest : MonoBehaviour
{
    [Header("Chest Contents")]
    public List<InventoryItem> chestItems = new List<InventoryItem>();

    [Header("References")]
    public InventorySO playerInventory;

    public ChestPage chestPage;  // Défini l'interface qui va etre utilisé à travers le script ChestPage

    private bool isOpen = false;

    public void Interact() // Intéraction change le mode du coffre (ouvert/fermé)
    {
        if (!isOpen)
            OpenChest();
        else
            CloseChest();      
    }

    private void OpenChest()    //Affiche L'interface et montre les items a l'interieur
    {
        isOpen = true;

        if (chestPage != null)
        {
            chestPage.gameObject.SetActive(true);
            chestPage.ShowChest(this);
        }
    }

    public void CloseChest()  //ferme l'interface
    {
        isOpen = false;

        if (chestPage != null)
            chestPage.HideChest();
    }


    public void TakeItem(int index)     // Permet de prendre un item du coffre, de l'enlever du coffre et de le mettre dans l'inventaire, si le joueur ne peut pas tout prendre laisse le reste.
    {
        if (index < 0 || index >= chestItems.Count)
            return;
        if (chestItems[index].IsEmpty)
            return;


        InventoryItem chestItem = chestItems[index];
        var itemSO = chestItem.item;
        int quantity = chestItem.quantity;


        int remainder = playerInventory.AddItem(itemSO, quantity, chestItem.itemState);


        int taken = quantity - remainder;


        if (taken >= chestItem.quantity)
        {
            chestItems[index] = InventoryItem.GetEmptyItem();
        }
        else
        {

            chestItems[index] = chestItems[index].ChangeQuantity(chestItem.quantity - taken);
        }


        if (chestPage != null)
            chestPage.RefreshChest(this);
    }
}
