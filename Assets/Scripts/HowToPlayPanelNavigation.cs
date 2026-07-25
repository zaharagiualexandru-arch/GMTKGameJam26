using UnityEngine;

public class HowToPlayPanelNavigation : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject watchPanel;
    [SerializeField] private GameObject hermesPanel;
    [SerializeField] private GameObject timeLockPanel;

    private void Start()
    {
        watchPanel.SetActive(false);
        hermesPanel.SetActive(false);
        timeLockPanel.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        TransitionToPanel(howToPlayPanel);
    }

    public void ShowWatch()
    {
        TransitionToPanel(watchPanel);
    }

    public void ShowHermes()
    {
        TransitionToPanel(hermesPanel);
    }

    public void ShowTimeLock()
    {
        TransitionToPanel(timeLockPanel);
    }

    private void TransitionToPanel(
        GameObject targetPanel)
    {
        if (PixelTransition.Instance != null)
        {
            PixelTransition.Instance.TransitionAction(
                () => SetActivePanel(targetPanel)
            );

            return;
        }

        SetActivePanel(targetPanel);
    }

    private void SetActivePanel(
        GameObject targetPanel)
    {
        howToPlayPanel.SetActive(false);
        watchPanel.SetActive(false);
        hermesPanel.SetActive(false);
        timeLockPanel.SetActive(false);

        targetPanel.SetActive(true);
    }
}