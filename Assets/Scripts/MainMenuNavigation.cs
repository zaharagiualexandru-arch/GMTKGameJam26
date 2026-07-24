using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNavigation : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void ReturnToMainMenu()
    {
        if (PixelTransition.Instance != null)
        {
            PixelTransition.Instance.TransitionToScene(
                mainMenuSceneName
            );

            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}