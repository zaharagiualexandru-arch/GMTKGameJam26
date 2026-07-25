using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHUD : MonoBehaviour
{
    [SerializeField]
    private CharacterStatusEffects playerEffects;

    [SerializeField] private Image hermesIcon;
    [SerializeField] private TMP_Text hermesTimer;

    [SerializeField] private Image timeLockIcon;
    [SerializeField] private TMP_Text timeLockTimer;

    [SerializeField] private float inactiveOpacity = 0.2f;
    [SerializeField] private float activeOpacity = 1f;

    private void Update()
    {
        UpdateAbility(
            hermesIcon,
            hermesTimer,
            playerEffects.SpeedBoostRemaining
        );

        UpdateAbility(
            timeLockIcon,
            timeLockTimer,
            playerEffects.TimeLockRemaining
        );
    }

    private void UpdateAbility(
        Image icon,
        TMP_Text timerText,
        float remainingDuration)
    {
        bool isActive =
            remainingDuration > 0f;

        Color iconColour = icon.color;

        iconColour.a = isActive
            ? activeOpacity
            : inactiveOpacity;

        icon.color = iconColour;

        timerText.gameObject.SetActive(isActive);

        if (isActive)
        {
            timerText.text =
                Mathf.CeilToInt(
                    remainingDuration
                ).ToString();
        }
    }
}