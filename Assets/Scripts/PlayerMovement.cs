using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterStatusEffects))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private CharacterStatusEffects statusEffects;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        statusEffects =
            GetComponent<CharacterStatusEffects>();
    }

    private void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }

    private void FixedUpdate()
    {
        float currentSpeed =
            moveSpeed *
            statusEffects.SpeedMultiplier;

        rb.linearVelocity =
            moveInput * currentSpeed;
    }
}