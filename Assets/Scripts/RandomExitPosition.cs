using UnityEngine;

public class RandomExitPosition : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private RectTransform exitLabel;

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

        if (exitLabel != null)
        {
            exitLabel.rotation = Quaternion.identity;
        }
    }
}