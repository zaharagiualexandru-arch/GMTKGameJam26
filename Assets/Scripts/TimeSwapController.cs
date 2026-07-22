using UnityEngine;

[RequireComponent(typeof(TimeHolder))]
public class TimeSwapController : MonoBehaviour
{
    [SerializeField] private float swapRange = 4f;
    [SerializeField] private float selectionRadius = 0.75f;
    [SerializeField] private float swapCooldown = 2f;

    public float CooldownRemaining =>
        Mathf.Max(0f, nextSwapTime - Time.time);

    private Camera mainCamera;
    private TimeHolder playerTime;
    private TimeHolder currentTarget;
    private SpriteRenderer highlightedRenderer;
    private Color originalColour;
    private float nextSwapTime;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerTime = GetComponent<TimeHolder>();
    }

    private void Update()
    {
        FindTarget();

        if (Input.GetMouseButtonDown(0) &&
            currentTarget != null &&
            CooldownRemaining <= 0f)
        {
            playerTime.SwapTime(currentTarget);
            nextSwapTime = Time.time + swapCooldown;
        }
    }

    private void FindTarget()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapCircleAll(mousePosition, selectionRadius);

        TimeHolder closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            TimeHolder candidate = hit.GetComponentInParent<TimeHolder>();

            if (candidate == null ||
                candidate == playerTime ||
                candidate.IsExpired)
            {
                continue;
            }

            float distanceFromPlayer = Vector2.Distance(
                transform.position,
                candidate.transform.position
            );

            if (distanceFromPlayer > swapRange)
            {
                continue;
            }

            float distanceFromMouse = Vector2.Distance(
                mousePosition,
                candidate.transform.position
            );

            if (distanceFromMouse < closestDistance)
            {
                closestDistance = distanceFromMouse;
                closestTarget = candidate;
            }
        }

        if (closestTarget != currentTarget)
        {
            SetTarget(closestTarget);
        }
    }

    private void SetTarget(TimeHolder target)
    {
        if (highlightedRenderer != null)
        {
            highlightedRenderer.color = originalColour;
        }

        currentTarget = target;
        highlightedRenderer = null;

        if (currentTarget == null)
        {
            return;
        }

        highlightedRenderer =
            currentTarget.GetComponentInChildren<SpriteRenderer>();

        if (highlightedRenderer != null)
        {
            originalColour = highlightedRenderer.color;
            highlightedRenderer.color = Color.yellow;
        }
    }

    private void OnDisable()
    {
        SetTarget(null);
    }
}