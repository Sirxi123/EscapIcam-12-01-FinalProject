using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private DoorController door; // Référence à la porte à contrôler
    [SerializeField] private DoorController doorOpened; // Référence à la porte ouverte
    [SerializeField] private bool isAutomatic = true; // Si true, la porte s'ouvre/ferme automatiquement
    [SerializeField] private KeyCode manualToggleKey; // Touche pour ouvrir/fermer manuellement

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isAutomatic && collider.CompareTag("Player"))
        {
            door.OpenDoor(); // Ouvre automatiquement la porte
            doorOpened.CloseDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (isAutomatic && collider.CompareTag("Player"))
        {
            door.CloseDoor(); // Ferme automatiquement la porte
            doorOpened.OpenDoor();
        }
    }

    private void Update()
    {
        // Gestion des interactions manuelles
        if (!isAutomatic && Input.GetKeyDown(manualToggleKey))
        {
            door.ToggleDoor(); // Inverse l'état de la porte
        }
    }
}
