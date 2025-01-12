using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChestTrigger2D : MonoBehaviour
{
    private bool isPlayerInRange = false;   

    private Chest chest;                   // Reference to the Chest script on this same object

    private void Awake()
    {
        
        chest = GetComponent<Chest>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            box.isTrigger = true; // Set the collider to be a trigger
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player enters
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the player exits 
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (chest != null)
            {
                chest.CloseChest();
            }
        }

    }

    private void Update()
    {
        // Only allow interaction if the player is inside the trigger
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Call the Chest’s Interact() method
            if (chest != null)
            {
                chest.Interact();
            }
        }
    }
}
