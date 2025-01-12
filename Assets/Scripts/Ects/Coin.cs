using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private int ectsGained = 1; // Points ECTS donnés
    [SerializeField] private AudioClip collectSound; // Son de collecte

    private CircleCollider2D circleCollider;
    private SpriteRenderer visual;
    private AudioSource audioSource;

    private void Awake()
    {
        // Initialisation des composants
        circleCollider = GetComponent<CircleCollider2D>();
        visual = GetComponentInChildren<SpriteRenderer>();

        // Ajout d'une source audio si elle n'existe pas déjà
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void CollectCoin()
    {
        // Désactivation des composants
        circleCollider.enabled = false;
        visual.enabled = false;  // Cache le sprite

        // Jouer le son de collecte
        if (collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        // Ajouter des ECTS
        EctsManager.Instance.AddEcts(ectsGained);

        // Détruire la pièce après la durée du son
        Destroy(gameObject, collectSound != null ? collectSound.length : 0f);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            CollectCoin();
        }
    }
}
