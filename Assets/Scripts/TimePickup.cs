using TMPro;
using UnityEngine;

public class TimePickup : MonoBehaviour
{
    [SerializeField] private float startingValue = 10f;
    [SerializeField] private float maximumTimeAfterPickup = 30f;
    [SerializeField] private TMP_Text valueText;

    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupVolume = 0.8f;

    public float RemainingValue { get; private set; }

    private bool collected;

    private void Awake()
    {
        RemainingValue = startingValue;
        UpdateText();
    }

    private void Update()
    {
        if (collected)
        {
            return;
        }

        RemainingValue = Mathf.Max(
            0f,
            RemainingValue - Time.deltaTime
        );

        UpdateText();

        if (RemainingValue <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public bool CanBeCollectedBy(TimeHolder collector)
    {
        return !collected &&
               RemainingValue > 0f &&
               collector != null &&
               !collector.IsExpired &&
               collector.RemainingTime <
               maximumTimeAfterPickup;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TimeHolder collector =
            other.GetComponentInParent<TimeHolder>();

        if (!CanBeCollectedBy(collector))
        {
            return;
        }

        collected = true;

        int displayedValue =
            Mathf.CeilToInt(RemainingValue);

        float availableSpace =
            maximumTimeAfterPickup -
            collector.RemainingTime;

        float gainedTime = Mathf.Min(
            displayedValue,
            availableSpace
        );

        collector.AddTime(gainedTime);

        if (collector.CompareTag("Player"))
        {
            PlayPickupSound();
        }

        Destroy(gameObject);
    }

    public void Initialize(float value)
    {
        RemainingValue = Mathf.Max(1f, value);
        UpdateText();
    }

    private void PlayPickupSound()
    {
        if (pickupSound == null)
        {
            return;
        }

        Vector3 soundPosition =
            Camera.main != null
                ? Camera.main.transform.position
                : transform.position;

        AudioSource.PlayClipAtPoint(
            pickupSound,
            soundPosition,
            pickupVolume
        );
    }

    private void UpdateText()
    {
        valueText.text =
            $"+{Mathf.CeilToInt(RemainingValue)}";
    }
}