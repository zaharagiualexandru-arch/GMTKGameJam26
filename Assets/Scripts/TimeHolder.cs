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
        RemainingTime = Mathf.Max(0f, startingTime);
    }

    private void Update()
    {
        if (IsPaused || expirationTriggered)
        {
            return;
        }

        SetTime(RemainingTime - Time.deltaTime);
    }

    public void SetTime(float value)
    {
        RemainingTime = Mathf.Max(0f, value);

        if (RemainingTime > 0f)
        {
            expirationTriggered = false;
            return;
        }

        TriggerExpiration();
    }

    public void AddTime(float amount)
    {
        SetTime(RemainingTime + Mathf.Max(0f, amount));
    }

    public float RemoveTime(float amount)
    {
        float removedTime = Mathf.Min(
            RemainingTime,
            Mathf.Max(0f, amount)
        );

        SetTime(RemainingTime - removedTime);
        return removedTime;
    }

    public void SwapTime(TimeHolder other)
    {
        float previousTime = RemainingTime;

        SetTime(other.RemainingTime);
        other.SetTime(previousTime);
    }

    private void TriggerExpiration()
    {
        if (expirationTriggered)
        {
            return;
        }

        expirationTriggered = true;
        Expired?.Invoke(this);
    }
}