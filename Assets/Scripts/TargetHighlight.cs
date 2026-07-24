using UnityEngine;

public class TargetHighlight : MonoBehaviour
{
    [SerializeField] private GameObject marker;

    private void Awake()
    {
        if (marker != null)
        {
            marker.SetActive(false);
        }
    }

    public void SetHighlighted(bool isHighlighted)
    {
        if (marker != null)
        {
            marker.SetActive(isHighlighted);
        }
    }

    private void OnDisable()
    {
        if (marker != null)
        {
            marker.SetActive(false);
        }
    }
}