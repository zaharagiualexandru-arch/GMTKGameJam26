using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private TimePickup watchPrefab;
    [SerializeField] private int initialCount = 3;
    [SerializeField] private int maximumActiveWatches = 4;

    [SerializeField]
    private Vector2 spawnIntervalRange =
        new Vector2(5f, 8f);

    [SerializeField]
    private Vector2 minimumBounds =
        new Vector2(-18.5f, -10.5f);

    [SerializeField]
    private Vector2 maximumBounds =
        new Vector2(18.5f, 10.5f);

    [SerializeField] private int minimumWatchValue = 10;
    [SerializeField] private int maximumWatchValue = 15;
    [SerializeField] private float spawnClearance = 0.75f;

    private readonly List<TimePickup> activeWatches =
        new List<TimePickup>();

    private float spawnTimer;

    private void Start()
    {
        for (int i = 0; i < initialCount; i++)
        {
            SpawnWatch();
        }

        ResetSpawnTimer();
    }

    private void Update()
    {
        activeWatches.RemoveAll(watch => watch == null);

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f)
        {
            return;
        }

        if (activeWatches.Count < maximumActiveWatches)
        {
            SpawnWatch();
        }

        ResetSpawnTimer();
    }

    private void SpawnWatch()
    {
        if (!TryFindSpawnPosition(out Vector2 position))
        {
            return;
        }

        TimePickup watch = Instantiate(
            watchPrefab,
            position,
            Quaternion.identity
        );

        int value = Random.Range(
            minimumWatchValue,
            maximumWatchValue + 1
        );

        watch.Initialize(value);
        activeWatches.Add(watch);
    }

    private bool TryFindSpawnPosition(out Vector2 position)
    {
        for (int attempt = 0; attempt < 25; attempt++)
        {
            Vector2 candidate = new Vector2(
                Random.Range(minimumBounds.x, maximumBounds.x),
                Random.Range(minimumBounds.y, maximumBounds.y)
            );

            if (Physics2D.OverlapCircle(
                    candidate,
                    spawnClearance) != null)
            {
                continue;
            }

            position = candidate;
            return true;
        }

        position = Vector2.zero;
        return false;
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(
            spawnIntervalRange.x,
            spawnIntervalRange.y
        );
    }
}