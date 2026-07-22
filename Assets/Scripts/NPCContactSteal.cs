using UnityEngine;

[RequireComponent(typeof(TimeHolder))]
public class NPCContactSteal : MonoBehaviour
{
    [SerializeField] private float stealAmount = 1f;
    [SerializeField] private float stealInterval = 0.75f;
    [SerializeField] private FloatingTimeText floatingTextPrefab;
    [SerializeField] private Vector3 textOffset = new Vector3(0f, 1.25f, 0f);

    [SerializeField]
    private Color gainedColour =
        new Color(0.1f, 0.7f, 0.2f);

    [SerializeField]
    private Color lostColour =
        new Color(0.9f, 0.1f, 0.1f);

    private TimeHolder npcTime;
    private float nextStealTime;

    private void Awake()
    {
        npcTime = GetComponent<TimeHolder>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time < nextStealTime)
        {
            return;
        }

        TimeHolder targetTime =
            collision.collider.GetComponentInParent<TimeHolder>();

        if (targetTime == null ||
            targetTime == npcTime ||
            targetTime.IsExpired ||
            npcTime.IsExpired)
        {
            return;
        }

        int npcSeconds = Mathf.CeilToInt(npcTime.RemainingTime);
        int targetSeconds = Mathf.CeilToInt(targetTime.RemainingTime);

        bool targetIsPlayer = targetTime.CompareTag("Player");

        bool canSteal =
            targetSeconds > npcSeconds ||
            (targetIsPlayer && targetSeconds == npcSeconds);

        if (!canSteal)
        {
            return;
        }

        Vector3 targetPosition = targetTime.transform.position;
        float stolenTime = targetTime.RemoveTime(stealAmount);

        if (stolenTime <= 0f)
        {
            return;
        }

        npcTime.AddTime(stolenTime);
        nextStealTime = Time.time + stealInterval;

        CreateFloatingText(
            transform.position,
            $"+{stolenTime:0.#}",
            gainedColour
        );

        CreateFloatingText(
            targetPosition,
            $"-{stolenTime:0.#}",
            lostColour
        );
    }

    private void CreateFloatingText(
        Vector3 position,
        string message,
        Color colour)
    {
        FloatingTimeText floatingText = Instantiate(
            floatingTextPrefab,
            position + textOffset,
            Quaternion.identity
        );

        floatingText.Show(message, colour);
    }
}