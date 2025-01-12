using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Nécessaire pour les coroutines

public class NpcDialog : MonoBehaviour
{
    public GameObject dialogBox;         // Boîte de dialogue
    public Text dialogText;              // Texte du dialogue
    public string dialog;                // Texte du dialogue à afficher
    public bool playerInRange;           // Si le joueur est dans la zone de trigger
    public KeyCode interactKey = KeyCode.E; // Touche d'interaction configurable dans l'éditeur

    public GameObject interactionText;   // Texte affiché pour "Appuyez sur E pour interagir"
    public string interactionMessage;  // Message d'interaction modifiable

    private void Update()
    {
        // Afficher ou cacher l'indicateur de texte pour interagir
        if (playerInRange)
        {
            // Si le dialogue n'est pas affiché, montrer le texte d'interaction
            if (!dialogBox.activeInHierarchy)
            { 
                // Mettre à jour le texte d'interaction en fonction de la variable interactionMessage
                interactionText.GetComponent<Text>().text = interactionMessage;
            }
        }

        // Si la touche d'interaction est pressée et que le joueur est dans la zone
        if (Input.GetKeyDown(interactKey) && playerInRange)
        {
            if (dialogBox.activeInHierarchy)
            {
                // Si le dialogue est déjà affiché, on le cache
                dialogBox.SetActive(false);
                interactionText.SetActive(true);
            }
            else
            {
                // Si le dialogue est caché, on l'affiche et on met le texte
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
            // Démarrer une coroutine pour attendre 0.5 secondes avant de fermer le dialogue
            if (dialogBox.activeInHierarchy)
            {
                StartCoroutine(WaitAndHideDialog(0.5f)); // Attendre 0.5 secondes avant de cacher le dialogue
            }
        }
    }

    // Coroutine pour cacher le dialogue après un délai
    private IEnumerator WaitAndHideDialog(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogBox.SetActive(false);
    }
}

