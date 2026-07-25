using TMPro;
using UnityEngine;

[RequireComponent(typeof(TimeHolder))]
public class CharacterStatusEffects : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;

    [SerializeField]
    private Color speedColour =
        new Color(0.95f, 0.75f, 0.1f);

    [SerializeField]
    private Color lockColour =
        new Color(0.1f, 0.55f, 1f);

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
        UpdateStatusText();
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

        UpdateStatusText();
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

        UpdateStatusText();
    }

    public void ApplyTimeLock(float duration)
    {
        timeLockRemaining = Mathf.Max(
            timeLockRemaining,
            duration
        );

        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        if (statusText == null)
        {
            return;
        }

        bool hasSpeed =
            speedBoostRemaining > 0f;

        bool hasLock =
            timeLockRemaining > 0f;

        statusText.gameObject.SetActive(
            hasSpeed || hasLock
        );

        if (!hasSpeed && !hasLock)
        {
            statusText.text = "";
            return;
        }

        string speedHex =
            ColorUtility.ToHtmlStringRGB(
                speedColour
            );

        string lockHex =
            ColorUtility.ToHtmlStringRGB(
                lockColour
            );

        string speedMessage = hasSpeed
            ? $"<color=#{speedHex}>SPEED {Mathf.CeilToInt(speedBoostRemaining)}</color>"
            : "";

        string lockMessage = hasLock
            ? $"<color=#{lockHex}>LOCK {Mathf.CeilToInt(timeLockRemaining)}</color>"
            : "";

        if (hasSpeed && hasLock)
        {
            statusText.text =
                $"{speedMessage}\n{lockMessage}";
        }
        else
        {
            statusText.text =
                hasSpeed
                    ? speedMessage
                    : lockMessage;
        }
    }
}