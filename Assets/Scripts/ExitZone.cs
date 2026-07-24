using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExitZone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer exitRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        SetOpen(false);
    }

    public void SetOpen(bool isOpen)
    {
        IsOpen = isOpen;

        exitRenderer.sprite =
            isOpen ? openSprite : closedSprite;

        exitRenderer.color = Color.white;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsOpen && other.CompareTag("Player"))
        {
            gameManager.PlayerEscaped();
        }
    }
}