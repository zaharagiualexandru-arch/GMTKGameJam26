using System;
using UnityEngine;

public class TimeHolder : MonoBehaviour
{
    [SerializeField] private float startingTime = 10f;

    public float RemainingTime { get; private set; }
    public bool IsExpired => RemainingTime <= 0f;
    public bool IsPaused { get; set; }

    public event Action<TimeHolder> Expired;

    private bool expirationTriggered;

    private void Awake()
    {
        RemainingTime = startingTime;
    }

    private void Update()
    {
        if (IsPaused || expirationTriggered)
        {
            return;
        }

        RemainingTime = Mathf.Max(0f, RemainingTime - Time.deltaTime);

        if (RemainingTime <= 0f)
        {
            expirationTriggered = true;
            Expired?.Invoke(this);
        }
    }

    public void SetTime(float value)
    {
        RemainingTime = Mathf.Max(0f, value);
        expirationTriggered = RemainingTime <= 0f;
    }

    public void AddTime(float amount)
    {
        SetTime(RemainingTime + amount);
    }

    public void SwapTime(TimeHolder other)
    {
        float previousTime = RemainingTime;
        SetTime(other.RemainingTime);
        other.SetTime(previousTime);
    }
}