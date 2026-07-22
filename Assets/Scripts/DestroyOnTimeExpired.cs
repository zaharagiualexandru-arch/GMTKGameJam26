using UnityEngine;

[RequireComponent(typeof(TimeHolder))]
public class DestroyOnTimeExpired : MonoBehaviour
{
    private TimeHolder timeHolder;

    private void Awake()
    {
        timeHolder = GetComponent<TimeHolder>();
        timeHolder.Expired += HandleExpired;
    }

    private void OnDestroy()
    {
        timeHolder.Expired -= HandleExpired;
    }

    private void HandleExpired(TimeHolder expiredTimeHolder)
    {
        Destroy(gameObject);
    }
}