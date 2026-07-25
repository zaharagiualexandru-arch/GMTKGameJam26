using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIButtonAudio : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private float clickVolume = 1f;
    [SerializeField] private float hoverVolume = 0.7f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    private void Start()
    {
        Button[] buttons =
            GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayClick);

            EventTrigger trigger =
                button.GetComponent<EventTrigger>();

            if (trigger == null)
            {
                trigger =
                    button.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry hoverEntry =
                new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };

            hoverEntry.callback.AddListener(
                _ => PlayHover()
            );

            trigger.triggers.Add(hoverEntry);
        }
    }

    private void PlayClick()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(
                clickSound,
                clickVolume
            );
        }
    }

    private void PlayHover()
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(
                hoverSound,
                hoverVolume
            );
        }
    }
}