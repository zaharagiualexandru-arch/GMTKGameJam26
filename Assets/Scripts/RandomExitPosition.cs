using UnityEngine;

public class RandomExitPosition : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        if (spawnPoints == null ||
            spawnPoints.Length == 0)
        {
            return;
        }

        Transform selectedPoint =
            spawnPoints[
                Random.Range(0, spawnPoints.Length)
            ];

        transform.SetPositionAndRotation(
            selectedPoint.position,
            selectedPoint.rotation
        );
    }
}