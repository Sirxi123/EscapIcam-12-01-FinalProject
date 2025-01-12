using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector2 change;  // Utilisation de Vector2 pour simplifier le code
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.gravityScale = 0; // Désactiver la gravité
    }

    void FixedUpdate()
    {
        // Récupérer les entrées utilisateur (horizontal et vertical)
        change = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        UpdateAnimationAndMove(change);
    }

    void UpdateAnimationAndMove(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            // Déplacer le personnage
            MoveCharacter(moveDirection);

            // Prioriser le mouvement horizontal (moveX) et ignorer moveY si moveX est différent de zéro
            animator.SetFloat("moveX", moveDirection.x);
            animator.SetFloat("moveY", (moveDirection.x != 0) ? 0 : moveDirection.y);  // Si moveX est non nul, set moveY à 0

            // Indiquer que le personnage se déplace
            animator.SetBool("moving", true);
        }
        else
        {
            // Si le joueur ne bouge pas
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter(Vector2 moveDirection)
    {
        // Déplacer le joueur avec MovePosition pour un mouvement physique
        myRigidbody.MovePosition((Vector2)transform.position + moveDirection.normalized * speed * Time.deltaTime);
    }
}
