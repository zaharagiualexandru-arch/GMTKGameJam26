using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private float movementThreshold = 0.05f;
    [SerializeField] private bool originalSpriteFacesRight = true;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 velocity = rb.linearVelocity;

        bool isMoving =
            velocity.sqrMagnitude >
            movementThreshold * movementThreshold;

        animator.SetBool("IsMoving", isMoving);

        if (!isMoving)
        {
            return;
        }

        bool movingHorizontally =
            Mathf.Abs(velocity.x) >=
            Mathf.Abs(velocity.y);

        if (movingHorizontally)
        {
            animator.SetInteger("Direction", 0);

            bool movingLeft = velocity.x < 0f;

            spriteRenderer.flipX =
                originalSpriteFacesRight
                    ? movingLeft
                    : !movingLeft;

            return;
        }

        spriteRenderer.flipX = false;

        if (velocity.y > 0f)
        {
            animator.SetInteger("Direction", 1);
        }
        else
        {
            animator.SetInteger("Direction", 2);
        }
    }
}