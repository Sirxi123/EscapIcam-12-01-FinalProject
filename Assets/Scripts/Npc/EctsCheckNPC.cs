using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Inventory.Model;
using Inventory;

public class EctsCheckNPC : MonoBehaviour
{
    [SerializeField] private int requiredEcts = 10; // Ects a avoir a modifier sur l'inspector
    [SerializeField] private List<ItemSO> rewardItems; // Liste d'items a donner
    [SerializeField] private List<int> rewardQuantities; // Quantité de chaque item

    [Header("UI Elements")]
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private string ectsNpcName = "???";

    [Header("Interaction")]
    public bool playerInRange;
    public KeyCode interactKey = KeyCode.E;

    private InventoryController playerInventory;
    private bool hasInteracted = false;
    private bool rewardGiven = false; // Permet de ne pas donner la récompense plusieur fois

    private void Start()
    {
        npcNameText.text = ectsNpcName;
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
            CheckEcts(); // Vérifie si le Joueur intéragit avec le personnage
        }
    }

    private void CheckEcts()
    {
        int currentEcts = EctsManager.Instance.CurrentEcts;
        dialogBox.SetActive(true);

        npcNameText.text = ectsNpcName; //Actualise l'interface avec le nom du Personnage
        questTitleText.text = "???";

        if (rewardGiven) 
        {
            dialogText.text = "Tu as déjà reçu ta récompense."; 
            return;
        }

        if (currentEcts >= requiredEcts)
        {
            GiveReward();
            dialogText.text = "Bravo... Voici ta récompense."; // Si on a plus d'ects qu'il ne le faut, on fini la quete.
            hasInteracted = true;
            rewardGiven = true; 
        }
        else if (!hasInteracted)
        {
            dialogText.text = "..."; 
        }
    }

    private void GiveReward()
    {
        for (int i = 0; i < rewardItems.Count; i++)
        {
            playerInventory.inventoryData.AddItem(rewardItems[i], rewardQuantities[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            npcNameText.text = ectsNpcName;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }
}
