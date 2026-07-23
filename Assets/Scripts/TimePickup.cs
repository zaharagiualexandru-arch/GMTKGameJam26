using TMPro;
using UnityEngine;

public class TimePickup : MonoBehaviour
{
    [SerializeField] private float startingValue = 10f;
    [SerializeField] private TMP_Text valueText;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        TimeHolder playerTime =
            other.GetComponentInParent<TimeHolder>();

        if (collected ||
            playerTime == null ||
            !playerTime.CompareTag("Player") ||
            playerTime.IsExpired)
        {
            return;
        }

        collected = true;

        int gainedSeconds =
            Mathf.CeilToInt(RemainingValue);

        playerTime.AddTime(gainedSeconds);

        Destroy(gameObject);
    }

    public void Initialize(float value)
    {
        RemainingValue = Mathf.Max(1f, value);
        UpdateText();
    }
    private void UpdateText()
    {
        valueText.text =
            $"+{Mathf.CeilToInt(RemainingValue)}";
    }
}