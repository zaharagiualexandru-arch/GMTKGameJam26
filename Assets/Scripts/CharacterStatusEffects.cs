using UnityEngine;

[RequireComponent(typeof(TimeHolder))]
public class CharacterStatusEffects : MonoBehaviour
{
    [SerializeField]
    private GameObject timeLockWorldIndicator;

    public float SpeedMultiplier =>
        speedBoostRemaining > 0f
            ? activeSpeedMultiplier
            : 1f;

    public bool IsTimeLocked =>
        timeLockRemaining > 0f;

    public float SpeedBoostRemaining =>
        speedBoostRemaining;

    public float TimeLockRemaining =>
        timeLockRemaining;

    private float activeSpeedMultiplier = 1f;
    private float speedBoostRemaining;
    private float timeLockRemaining;

    private void Awake()
    {
        UpdateTimeLockIndicator();
    }

    private void Update()
    {
        if (speedBoostRemaining > 0f)
        {
            speedBoostRemaining = Mathf.Max(
                0f,
                speedBoostRemaining - Time.deltaTime
            );

            if (speedBoostRemaining <= 0f)
            {
                activeSpeedMultiplier = 1f;
            }
        }

        if (timeLockRemaining > 0f)
        {
            timeLockRemaining = Mathf.Max(
                0f,
                timeLockRemaining - Time.deltaTime
            );
        }

        UpdateTimeLockIndicator();
    }

    public void ApplySpeedBoost(
        float multiplier,
        float duration)
    {
        activeSpeedMultiplier = Mathf.Max(
            activeSpeedMultiplier,
            multiplier
        );

        speedBoostRemaining = Mathf.Max(
            speedBoostRemaining,
            duration
        );
    }

    public void ApplyTimeLock(float duration)
    {
        timeLockRemaining = Mathf.Max(
            timeLockRemaining,
            duration
        );

        UpdateTimeLockIndicator();
    }

    private void UpdateTimeLockIndicator()
    {
        if (timeLockWorldIndicator != null)
        {
            timeLockWorldIndicator.SetActive(
                IsTimeLocked
            );
        }
    }
}