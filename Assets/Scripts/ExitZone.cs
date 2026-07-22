using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExitZone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer exitRenderer;

    [SerializeField]
    private Color closedColour =
        new Color(0.25f, 0.25f, 0.25f);

    [SerializeField]
    private Color openColour =
        new Color(0.1f, 0.75f, 0.25f);

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        SetOpen(false);
    }

    public void SetOpen(bool isOpen)
    {
        IsOpen = isOpen;
        exitRenderer.color = isOpen ? openColour : closedColour;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (IsOpen && other.CompareTag("Player"))
        {
            gameManager.PlayerEscaped();
        }
    }
}