using UnityEngine;

public class TargetHighlight : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mainRenderer;
    [SerializeField]
    private Color outlineColor =
        new Color(0.5f, 0.03f, 0.03f, 1f);
    [SerializeField] private float outlineDistance = 0.035f;

    private SpriteRenderer[] outlineRenderers;

    private readonly Vector2[] outlineDirections =
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right,
        new Vector2(1f, 1f),
        new Vector2(1f, -1f),
        new Vector2(-1f, 1f),
        new Vector2(-1f, -1f)
    };

    private void Awake()
    {
        CreateOutline();
        SetHighlighted(false);
    }

    private void LateUpdate()
    {
        if (mainRenderer == null)
        {
            return;
        }

        foreach (SpriteRenderer outline in outlineRenderers)
        {
            outline.sprite = mainRenderer.sprite;
            outline.flipX = mainRenderer.flipX;
            outline.flipY = mainRenderer.flipY;
            outline.color = outlineColor;
            outline.sortingLayerID =
                mainRenderer.sortingLayerID;
            outline.sortingOrder =
                mainRenderer.sortingOrder - 1;
        }
    }

    private void CreateOutline()
    {
        outlineRenderers =
            new SpriteRenderer[outlineDirections.Length];

        for (int i = 0; i < outlineDirections.Length; i++)
        {
            GameObject outlineObject =
                new GameObject($"Outline_{i}");

            outlineObject.transform.SetParent(transform);
            outlineObject.transform.localPosition =
                outlineDirections[i].normalized * outlineDistance;
            outlineObject.transform.localRotation =
                Quaternion.identity;
            outlineObject.transform.localScale =
                Vector3.one;

            SpriteRenderer outline =
                outlineObject.AddComponent<SpriteRenderer>();

            outline.sprite = mainRenderer.sprite;
            outline.color = outlineColor;
            outline.sharedMaterial =
                mainRenderer.sharedMaterial;
            outline.sortingLayerID =
                mainRenderer.sortingLayerID;
            outline.sortingOrder =
                mainRenderer.sortingOrder - 1;

            outlineRenderers[i] = outline;
        }
    }

    public void SetHighlighted(bool isHighlighted)
    {
        if (outlineRenderers == null)
        {
            return;
        }

        foreach (SpriteRenderer outline in outlineRenderers)
        {
            outline.enabled = isHighlighted;
        }
    }
}