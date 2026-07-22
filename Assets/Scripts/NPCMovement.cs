using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TimeHolder))]
public class NPCMovement : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float fleeSpeed = 3f;

    private Rigidbody2D rb;
    private TimeHolder timeHolder;
    private Transform player;
    private TimeHolder playerTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        timeHolder = GetComponent<TimeHolder>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        player = playerObject.transform;
        playerTime = playerObject.GetComponent<TimeHolder>();
    }

    private void FixedUpdate()
    {
        if (timeHolder.IsExpired || playerTime.IsExpired)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 directionToPlayer =
            ((Vector2)player.position - rb.position).normalized;

        int npcSeconds = Mathf.CeilToInt(timeHolder.RemainingTime);
        int playerSeconds = Mathf.CeilToInt(playerTime.RemainingTime);

        bool shouldFlee = npcSeconds > playerSeconds;

        if (shouldFlee)
        {
            rb.linearVelocity = -directionToPlayer * fleeSpeed;
        }
        else
        {
            rb.linearVelocity = directionToPlayer * chaseSpeed;
        }
    }
}