using System.Collections.Generic;
using UnityEngine;

public class AbilitySpawner : MonoBehaviour
{
    [SerializeField]
    private AbilityPickup[] abilityPrefabs;

    [SerializeField]
    private Vector2 minimumBounds =
        new Vector2(-18f, -10f);

    [SerializeField]
    private Vector2 maximumBounds =
        new Vector2(18f, 10f);

    [SerializeField]
    private Vector2 initialSpawnDelayRange =
        new Vector2(5f, 8f);

    [SerializeField]
    private Vector2 spawnIntervalRange =
        new Vector2(10f, 15f);

    [SerializeField] private int maximumActiveAbilities = 2;

    private readonly List<AbilityPickup> activeAbilities =
        new List<AbilityPickup>();

    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime =
            Time.time +
            Random.Range(
                initialSpawnDelayRange.x,
                initialSpawnDelayRange.y
            );
    }

    private void Update()
    {
        if (Time.time < nextSpawnTime)
        {
            return;
        }

        RemoveDestroyedAbilities();

        if (activeAbilities.Count <
            maximumActiveAbilities)
        {
            SpawnAbility();
        }

        nextSpawnTime =
            Time.time +
            Random.Range(
                spawnIntervalRange.x,
                spawnIntervalRange.y
            );
    }

    private void SpawnAbility()
    {
        if (abilityPrefabs == null ||
            abilityPrefabs.Length == 0)
        {
            return;
        }

        AbilityPickup selectedPrefab =
            abilityPrefabs[
                Random.Range(
                    0,
                    abilityPrefabs.Length
                )
            ];

        if (selectedPrefab == null)
        {
            return;
        }

        Vector2 spawnPosition = new Vector2(
            Random.Range(
                minimumBounds.x,
                maximumBounds.x
            ),
            Random.Range(
                minimumBounds.y,
                maximumBounds.y
            )
        );

        AbilityPickup spawnedAbility = Instantiate(
            selectedPrefab,
            spawnPosition,
            Quaternion.identity
        );

        activeAbilities.Add(spawnedAbility);
    }

    private void RemoveDestroyedAbilities()
    {
        for (int i = activeAbilities.Count - 1;
             i >= 0;
             i--)
        {
            if (activeAbilities[i] == null)
            {
                activeAbilities.RemoveAt(i);
            }
        }
    }
}