using TMPro;
using UnityEngine;

[RequireComponent(typeof(TimeHolder))]
public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private TimeHolder timeHolder;

    private void Awake()
    {
        timeHolder = GetComponent<TimeHolder>();
    }

    private void Update()
    {
        float remainingTime = timeHolder.RemainingTime;

        timerText.text = Mathf.CeilToInt(remainingTime).ToString();

        if (remainingTime <= 3f)
        {
            timerText.color = Color.red;
        }
        else if (remainingTime <= 6f)
        {
            timerText.color = new Color(1f, 0.5f, 0f);
        }
        else
        {
            timerText.color = Color.black;
        }
    }
}