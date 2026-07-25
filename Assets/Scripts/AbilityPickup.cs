using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AbilityPickup : MonoBehaviour
{
    private enum AbilityType
    {
        HermesBoot,
        TimeLock
    }

    [SerializeField] private AbilityType abilityType;
    [SerializeField] private float despawnDuration = 10f;
    [SerializeField] private float effectDuration = 4f;
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private TMP_Text countdownText;

    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupVolume = 0.8f;

    public float RemainingLifetime { get; private set; }

    private bool collected;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;

        RemainingLifetime = despawnDuration;
        UpdateCountdownText();
    }

    private void Update()
    {
        if (collected)
        {
            return;
        }

        RemainingLifetime = Mathf.Max(
            0f,
            RemainingLifetime - Time.deltaTime
        );

        UpdateCountdownText();

        if (RemainingLifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public bool CanBeCollectedBy(
        TimeHolder collector)
    {
        return !collected &&
               RemainingLifetime > 0f &&
               collector != null &&
               !collector.IsExpired &&
               collector.GetComponent<
                   CharacterStatusEffects>() != null;
    }

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        TimeHolder collector =
            other.GetComponentInParent<TimeHolder>();

        if (!CanBeCollectedBy(collector))
        {
            return;
        }

        CharacterStatusEffects effects =
            collector.GetComponent<
                CharacterStatusEffects>();

        collected = true;

        if (abilityType ==
            AbilityType.HermesBoot)
        {
            effects.ApplySpeedBoost(
                speedMultiplier,
                effectDuration
            );
        }
        else
        {
            effects.ApplyTimeLock(
                effectDuration
            );
        }

        if (collector.CompareTag("Player"))
        {
            PlayPickupSound();
        }

        Destroy(gameObject);
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

    private void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text =
                Mathf.CeilToInt(
                    RemainingLifetime
                ).ToString();
        }
    }
}