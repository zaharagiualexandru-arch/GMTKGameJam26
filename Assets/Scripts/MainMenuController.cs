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
        ShowMainMenu();
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowHowToPlay()
    {
        mainMenuContent.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        howToPlayPanel.SetActive(false);
        mainMenuContent.SetActive(true);
    }
}