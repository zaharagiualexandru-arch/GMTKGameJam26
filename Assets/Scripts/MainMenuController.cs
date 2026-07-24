using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "MainScene";
    [SerializeField] private GameObject mainMenuContent;
    [SerializeField] private GameObject howToPlayPanel;

    private void Start()
    {
        Time.timeScale = 1f;
        SetMenuState(true, false);
    }

    public void PlayGame()
    {
        if (PixelTransition.Instance != null)
        {
            PixelTransition.Instance.TransitionToScene(
                gameSceneName
            );

            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowHowToPlay()
    {
        TransitionMenuState(false, true);
    }

    public void ShowMainMenu()
    {
        TransitionMenuState(true, false);
    }

    private void TransitionMenuState(
        bool showMenu,
        bool showInstructions)
    {
        if (PixelTransition.Instance != null)
        {
            PixelTransition.Instance.TransitionAction(
                () => SetMenuState(
                    showMenu,
                    showInstructions
                )
            );

            return;
        }

        SetMenuState(showMenu, showInstructions);
    }

    private void SetMenuState(
        bool showMenu,
        bool showInstructions)
    {
        mainMenuContent.SetActive(showMenu);
        howToPlayPanel.SetActive(showInstructions);
    }
}