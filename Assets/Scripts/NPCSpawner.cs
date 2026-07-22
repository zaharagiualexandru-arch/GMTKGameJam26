using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private int initialCount = 18;
    [SerializeField] private int maximumNPCs = 22;
    [SerializeField] private float spawnInterval = 2f;

    [SerializeField]
    private Vector2 minimumBounds =
        new Vector2(-18.5f, -10.5f);

    [SerializeField]
    private Vector2 maximumBounds =
        new Vector2(18.5f, 10.5f);

    [SerializeField] private int minimumStartingTime = 5;
    [SerializeField] private int maximumStartingTime = 25;
    [SerializeField] private float minimumDistanceFromPlayer = 4f;
    [SerializeField] private float spawnClearance = 0.8f;

    private readonly List<GameObject> spawnedNPCs =
        new List<GameObject>();

    private Transform player;
    private float spawnTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < initialCount; i++)
        {
            SpawnNPC();
        }
    }

    private void Update()
    {
        spawnedNPCs.RemoveAll(npc => npc == null);

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f && spawnedNPCs.Count < maximumNPCs)
        {
            SpawnNPC();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnNPC()
    {
        if (!TryFindSpawnPosition(out Vector2 spawnPosition))
        {
            return;
        }

        GameObject npc = Instantiate(
            npcPrefab,
            spawnPosition,
            Quaternion.identity
        );

        int startingTime = Random.Range(
            minimumStartingTime,
            maximumStartingTime + 1
        );

        npc.GetComponent<TimeHolder>().SetTime(startingTime);
        spawnedNPCs.Add(npc);
    }

    private bool TryFindSpawnPosition(out Vector2 spawnPosition)
    {
        for (int attempt = 0; attempt < 25; attempt++)
        {
            Vector2 position = new Vector2(
                Random.Range(minimumBounds.x, maximumBounds.x),
                Random.Range(minimumBounds.y, maximumBounds.y)
            );

            if (Vector2.Distance(position, player.position) <
                minimumDistanceFromPlayer)
            {
                continue;
            }

            if (Physics2D.OverlapCircle(position, spawnClearance) != null)
            {
                continue;
            }

            spawnPosition = position;
            return true;
        }

        spawnPosition = Vector2.zero;
        return false;
    }
}