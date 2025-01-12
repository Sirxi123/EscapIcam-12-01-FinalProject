using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private bool isOpenInitially = false; // État initial de la porte

    private void Start()
    {
        // Configure l'état initial de la porte
        gameObject.SetActive(!isOpenInitially);
    }

    // Ouvrir la porte
    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    // Fermer la porte
    public void CloseDoor()
    {
        gameObject.SetActive(true);
    }

    // Inverser l'état de la porte
    public void ToggleDoor()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
