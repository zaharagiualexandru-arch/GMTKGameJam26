using TMPro;
using UnityEngine;

[RequireComponent(typeof(TimeHolder))]
public class TimeSwapController : MonoBehaviour
{
    [SerializeField] private float swapRange = 4f;
    [SerializeField] private float selectionRadius = 0.75f;
    [SerializeField] private float swapCooldown = 2f;
    [SerializeField] private TMP_Text cooldownText;

    [SerializeField] private AudioClip swapSound;
    [SerializeField] private float swapVolume = 0.9f;

    [SerializeField]
    private Color readyColour =
        new Color(0.1f, 0.75f, 0.25f);

    [SerializeField]
    private Color cooldownColour =
        new Color(1f, 0.55f, 0f);

    public float CooldownRemaining =>
        Mathf.Max(0f, nextSwapTime - Time.time);

    private Camera mainCamera;
    private TimeHolder playerTime;
    private TimeHolder currentTarget;
    private TargetHighlight currentHighlight;
    private AudioSource audioSource;

    private float nextSwapTime;
    private bool targetInRange;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerTime = GetComponent<TimeHolder>();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource =
                gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    private void Update()
    {
        FindTarget();
        UpdateTargetRange();

        bool canSwap =
            currentTarget != null &&
            targetInRange &&
            CooldownRemaining <= 0f;

        if (Input.GetMouseButtonDown(0) && canSwap)
        {
            playerTime.SwapTime(currentTarget);
            nextSwapTime = Time.time + swapCooldown;

            PlaySwapSound();
        }

        UpdateHighlight();
        UpdateCooldownText();
    }

    private void FindTarget()
    {
        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                mousePosition,
                selectionRadius
            );

        TimeHolder closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            TimeHolder candidate =
                hit.GetComponentInParent<TimeHolder>();

            if (candidate == null ||
                candidate == playerTime ||
                candidate.IsExpired)
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

    private void UpdateTargetRange()
    {
        if (currentTarget == null)
        {
            targetInRange = false;
            return;
        }

        targetInRange = Vector2.Distance(
            transform.position,
            currentTarget.transform.position
        ) <= swapRange;
    }

    private void UpdateHighlight()
    {
        if (currentHighlight == null)
        {
            return;
        }

        bool canSwap =
            targetInRange &&
            CooldownRemaining <= 0f;

        currentHighlight.SetHighlighted(canSwap);
    }

    private void UpdateCooldownText()
    {
        if (CooldownRemaining > 0f)
        {
            cooldownText.text =
                $"SWAP: {CooldownRemaining:0.0}";

            cooldownText.color = cooldownColour;
        }
        else
        {
            cooldownText.text = "SWAP: READY";
            cooldownText.color = readyColour;
        }
    }

    private void PlaySwapSound()
    {
        if (swapSound == null)
        {
            return;
        }

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(
            swapSound,
            swapVolume
        );
    }

    private void SetTarget(TimeHolder target)
    {
        if (currentHighlight != null)
        {
            currentHighlight.SetHighlighted(false);
        }

        currentTarget = target;
        currentHighlight = null;

        if (currentTarget != null)
        {
            currentHighlight =
                currentTarget.GetComponent<TargetHighlight>();
        }
    }

    private void OnDisable()
    {
        SetTarget(null);
    }
}