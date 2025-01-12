using UnityEngine;
using Inventory;
using Inventory.Model;
using System.Collections;

public class LockedDoor : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject doorClosed;  // La porte fermée
    [SerializeField] private GameObject doorOpened;  // La porte ouverte
    [SerializeField] private InventoryController playerInventory;  // Inventaire du joueur
    [SerializeField] private ItemSO requiredKey;  // Objet nécessaire pour ouvrir la porte
    [SerializeField] private GameObject interactionText;
    [SerializeField] private GameObject doorIsClosedText;

    [Header("Paramètres")]
    [SerializeField] private KeyCode openKey = KeyCode.E;  // Touche pour ouvrir la porte
    [SerializeField] private bool isAutomatic = false;  // Si vrai, la porte s'ouvre dès qu'on entre en contact

    private bool playerInRange = false;
    private bool isOpen = false;

    private void Start()
    {
        // Assurer que la porte commence bien dans l'état fermé
        doorClosed.SetActive(true);
        doorOpened.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(openKey) && !isOpen)
        {
            TryOpenDoor();
        }
    }

    private void TryOpenDoor()
    {
        if (playerInventory != null && requiredKey != null)
        {
            bool hasKey = false;

            // Vérifier si l'objet requis est présent dans l'inventaire
            foreach (var item in playerInventory.inventoryData.GetCurrentInventoryState())
            {
                if (item.Value.item == requiredKey)
                {
                    hasKey = true;
                    break;
                }
            }

            if (hasKey)
            {
                if (!isOpen)
                {
                    OpenDoor();
                }
            }
            else
            {
                doorIsClosedText.SetActive(true);
                StartCoroutine(WaitAndHideDialog(1.3f));
            }
        }
    }

    private IEnumerator WaitAndHideDialog(float delay)
    {
        yield return new WaitForSeconds(delay);
        doorIsClosedText.SetActive(false);
    }

    private void OpenDoor()
    {
        doorClosed.SetActive(false);
        doorOpened.SetActive(true);
        isOpen = true;

        // Disable interaction to prevent closing the door again
        interactionText.SetActive(false);
        playerInRange = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpen)  // Interaction only if the door is closed
        {
            playerInRange = true;
            interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionText.SetActive(false);
        }
    }
}
