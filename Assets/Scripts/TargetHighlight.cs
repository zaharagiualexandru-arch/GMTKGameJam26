using UnityEngine;

public class TargetHighlight : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mainRenderer;
    [SerializeField] private SpriteRenderer outlineRenderer;

    private void Awake()
    {
        outlineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        outlineRenderer.sprite = mainRenderer.sprite;
        outlineRenderer.flipX = mainRenderer.flipX;
        outlineRenderer.flipY = mainRenderer.flipY;
    }

    public void SetHighlighted(bool isHighlighted)
    {
        outlineRenderer.enabled = isHighlighted;
    }
}