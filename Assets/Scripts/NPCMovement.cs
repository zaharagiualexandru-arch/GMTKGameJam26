using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TimeHolder))]
public class NPCMovement : MonoBehaviour
{
    private enum MovementState
    {
        Wander,
        Chase,
        CollectPickup,
        Flee
    }

    [SerializeField] private float wanderSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.2f;
    [SerializeField] private float fleeSpeed = 4f;

    [SerializeField] private float awarenessRadius = 7f;
    [SerializeField] private float dangerRadius = 4.5f;
    [SerializeField] private float targetRefreshInterval = 0.25f;

    [SerializeField]
    private Vector2 minimumBounds =
        new Vector2(-19.5f, -11.5f);

    [SerializeField]
    private Vector2 maximumBounds =
        new Vector2(19.5f, 11.5f);

    [SerializeField] private float wallAvoidanceDistance = 2.5f;
    [SerializeField] private float wallAvoidanceStrength = 6f;

    [SerializeField]
    private Vector2 wanderIntervalRange =
        new Vector2(1.2f, 2.5f);

    private Rigidbody2D rb;
    private TimeHolder timeHolder;

    private TimeHolder chaseTarget;
    private TimeHolder fleeTarget;
    private TimeHolder touchingTarget;
    private TimePickup pickupTarget;
    private MovementState movementState;

    private Vector2 wanderDirection;
    private float nextWanderChange;
    private float nextTargetRefresh;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        timeHolder = GetComponent<TimeHolder>();
    }

    private void Start()
    {
        PickWanderDirection();

        nextTargetRefresh =
            Time.time + Random.Range(0f, targetRefreshInterval);
    }

    private void Update()
    {
        if (Time.time >= nextTargetRefresh)
        {
            RefreshTargets();
            nextTargetRefresh =
                Time.time + targetRefreshInterval;
        }

        if (movementState == MovementState.Wander &&
            Time.time >= nextWanderChange)
        {
            PickWanderDirection();
        }
    }

    private void FixedUpdate()
    {
        if (timeHolder.IsExpired)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 moveDirection = wanderDirection;
        float movementSpeed = wanderSpeed;

        if (movementState == MovementState.Chase &&
            chaseTarget != null)
        {
            if (touchingTarget == chaseTarget)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            moveDirection =
                ((Vector2)chaseTarget.transform.position -
                 rb.position).normalized;

            movementSpeed = chaseSpeed;
        }
        else if (
            movementState == MovementState.CollectPickup &&
            pickupTarget != null)
        {
            moveDirection =
                ((Vector2)pickupTarget.transform.position -
                 rb.position).normalized;

            movementSpeed = chaseSpeed;
        }
        else if (movementState == MovementState.Flee &&
                 fleeTarget != null)
        {
            moveDirection =
                (rb.position -
                 (Vector2)fleeTarget.transform.position)
                .normalized;

            movementSpeed = fleeSpeed;
        }

        moveDirection =
            ApplyWallAvoidance(moveDirection);

        rb.linearVelocity =
            moveDirection.normalized * movementSpeed;
    }

    private void RefreshTargets()
    {
        chaseTarget = null;
        fleeTarget = null;
        pickupTarget = null;

        int ownSeconds =
            Mathf.CeilToInt(timeHolder.RemainingTime);

        float bestTimeSourceScore = float.MinValue;
        float nearestThreatDistance = float.MaxValue;

        Collider2D[] nearbyColliders =
            Physics2D.OverlapCircleAll(
                rb.position,
                awarenessRadius
            );

        foreach (Collider2D nearbyCollider in nearbyColliders)
        {
            TimePickup pickup =
                nearbyCollider.GetComponentInParent<TimePickup>();

            if (pickup != null &&
                pickup.CanBeCollectedBy(timeHolder))
            {
                float pickupDistance =
                    Vector2.Distance(
                        rb.position,
                        pickup.transform.position
                    );

                float pickupScore =
                    pickup.RemainingValue /
                    (pickupDistance + 1f);

                if (pickupScore > bestTimeSourceScore)
                {
                    pickupTarget = pickup;
                    chaseTarget = null;
                    bestTimeSourceScore = pickupScore;
                }

                continue;
            }

            TimeHolder candidate =
                nearbyCollider.GetComponentInParent<TimeHolder>();

            if (candidate == null ||
                candidate == timeHolder ||
                candidate.IsExpired)
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    rb.position,
                    candidate.transform.position
                );

            int candidateSeconds =
                Mathf.CeilToInt(
                    candidate.RemainingTime
                );

            bool candidateIsPlayer =
                candidate.CompareTag("Player");

            bool canChase =
                candidateSeconds > ownSeconds ||
                (candidateIsPlayer &&
                 candidateSeconds == ownSeconds);

            if (canChase)
            {
                float potentialGain =
                    Mathf.Max(
                        1f,
                        candidate.RemainingTime -
                        timeHolder.RemainingTime
                    );

                float characterScore =
                    potentialGain / (distance + 1f);

                if (characterScore >
                    bestTimeSourceScore)
                {
                    chaseTarget = candidate;
                    pickupTarget = null;
                    bestTimeSourceScore =
                        characterScore;
                }
            }

            bool isThreat =
                candidateSeconds < ownSeconds &&
                distance <= dangerRadius;

            if (isThreat &&
                distance < nearestThreatDistance)
            {
                fleeTarget = candidate;
                nearestThreatDistance = distance;
            }
        }

        if (fleeTarget != null)
        {
            movementState = MovementState.Flee;
        }
        else if (pickupTarget != null)
        {
            movementState =
                MovementState.CollectPickup;
        }
        else if (chaseTarget != null)
        {
            movementState = MovementState.Chase;
        }
        else
        {
            movementState = MovementState.Wander;
        }
    }

    private Vector2 ApplyWallAvoidance(
        Vector2 direction)
    {
        Vector2 avoidance = Vector2.zero;

        avoidance.x += Mathf.Clamp01(
            (minimumBounds.x +
             wallAvoidanceDistance -
             rb.position.x) /
            wallAvoidanceDistance
        );

        avoidance.x -= Mathf.Clamp01(
            (rb.position.x -
             (maximumBounds.x -
              wallAvoidanceDistance)) /
            wallAvoidanceDistance
        );

        avoidance.y += Mathf.Clamp01(
            (minimumBounds.y +
             wallAvoidanceDistance -
             rb.position.y) /
            wallAvoidanceDistance
        );

        avoidance.y -= Mathf.Clamp01(
            (rb.position.y -
             (maximumBounds.y -
              wallAvoidanceDistance)) /
            wallAvoidanceDistance
        );

        return direction +
               avoidance * wallAvoidanceStrength;
    }

    private void OnCollisionStay2D(
        Collision2D collision)
    {
        TimeHolder otherTime =
            collision.gameObject
                .GetComponentInParent<TimeHolder>();

        if (otherTime == chaseTarget)
        {
            touchingTarget = otherTime;
        }
    }

    private void OnCollisionExit2D(
        Collision2D collision)
    {
        TimeHolder otherTime =
            collision.gameObject
                .GetComponentInParent<TimeHolder>();

        if (otherTime == touchingTarget)
        {
            touchingTarget = null;
        }
    }

    private void PickWanderDirection()
    {
        wanderDirection =
            Random.insideUnitCircle.normalized;

        if (wanderDirection == Vector2.zero)
        {
            wanderDirection = Vector2.right;
        }

        nextWanderChange =
            Time.time + Random.Range(
                wanderIntervalRange.x,
                wanderIntervalRange.y
            );
    }
}