using UnityEngine;
using UnityEngine.UI;
using System.Collections; // N�cessaire pour les coroutines

public class NpcDialog : MonoBehaviour
{
    public GameObject dialogBox;         // Bo�te de dialogue
    public Text dialogText;              // Texte du dialogue
    public string dialog;                // Texte du dialogue � afficher
    public bool playerInRange;           // Si le joueur est dans la zone de trigger
    public KeyCode interactKey = KeyCode.E; // Touche d'interaction configurable dans l'�diteur

    public GameObject interactionText;   // Texte affich� pour "Appuyez sur E pour interagir"
    public string interactionMessage;  // Message d'interaction modifiable

    private void Update()
    {
        // Afficher ou cacher l'indicateur de texte pour interagir
        if (playerInRange)
        {
            // Si le dialogue n'est pas affich�, montrer le texte d'interaction
            if (!dialogBox.activeInHierarchy)
            { 
                // Mettre � jour le texte d'interaction en fonction de la variable interactionMessage
                interactionText.GetComponent<Text>().text = interactionMessage;
            }
        }

        // Si la touche d'interaction est press�e et que le joueur est dans la zone
        if (Input.GetKeyDown(interactKey) && playerInRange)
        {
            if (dialogBox.activeInHierarchy)
            {
                // Si le dialogue est d�j� affich�, on le cache
                dialogBox.SetActive(false);
                interactionText.SetActive(true);
            }
            else
            {
                // Si le dialogue est cach�, on l'affiche et on met le texte
                dialogBox.SetActive(true);
                dialogText.text = dialog;
                interactionText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
            // D�marrer une coroutine pour attendre 0.5 secondes avant de fermer le dialogue
            if (dialogBox.activeInHierarchy)
            {
                StartCoroutine(WaitAndHideDialog(0.5f)); // Attendre 0.5 secondes avant de cacher le dialogue
            }
        }
    }

    // Coroutine pour cacher le dialogue apr�s un d�lai
    private IEnumerator WaitAndHideDialog(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogBox.SetActive(false);
    }
}

