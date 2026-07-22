using TMPro;
using UnityEngine;

public class FloatingTimeText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float riseDistance = 0.8f;

    private Vector3 startingPosition;
    private Color textColour;
    private float elapsedTime;
    private bool isActive;

    public void Show(string message, Color colour)
    {
        text.text = message;
        textColour = colour;
        startingPosition = transform.position;
        elapsedTime = 0f;
        isActive = true;

        Color invisibleColour = textColour;
        invisibleColour.a = 0f;
        text.color = invisibleColour;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        float progress = Mathf.Clamp01(elapsedTime / duration);

        transform.position =
            startingPosition + Vector3.up * riseDistance * progress;

        float alpha;

        if (progress < 0.2f)
        {
            alpha = progress / 0.2f;
        }
        else if (progress < 0.55f)
        {
            alpha = 1f;
        }
        else
        {
            alpha = 1f - ((progress - 0.55f) / 0.45f);
        }

        Color currentColour = textColour;
        currentColour.a = Mathf.Clamp01(alpha);
        text.color = currentColour;

        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }
}