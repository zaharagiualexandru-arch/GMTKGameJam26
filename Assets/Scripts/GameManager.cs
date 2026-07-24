using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float evacuationDuration = 60f;
    [SerializeField] private TMP_Text evacuationTimerText;
    [SerializeField] private TMP_Text startCountdownText;
    [SerializeField] private int startingCountdown = 3;
    [SerializeField] private float goDisplayDuration = 0.5f;
    [SerializeField] private ExitZone exitZone;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject victoryPanel;

    private TimeHolder playerTime;
    private PlayerMovement playerMovement;
    private TimeSwapController timeSwapController;

    private float evacuationTimeRemaining;
    private bool roundStarted;
    private bool exitOpened;
    private bool roundEnded;

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    private void Start()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        playerTime = player.GetComponent<TimeHolder>();
        playerMovement = player.GetComponent<PlayerMovement>();
        timeSwapController =
            player.GetComponent<TimeSwapController>();

        playerTime.Expired += HandlePlayerExpired;

        evacuationTimeRemaining = evacuationDuration;
        deathPanel.SetActive(false);
        victoryPanel.SetActive(false);
        exitZone.SetOpen(false);

        UpdateTimerText();
        SetPlayerControls(false);

        StartCoroutine(BeginRoundCountdown());
    }

    private void Update()
    {
        if (!roundStarted || roundEnded || exitOpened)
        {
            return;
        }

        evacuationTimeRemaining = Mathf.Max(
            0f,
            evacuationTimeRemaining - Time.deltaTime
        );

        if (evacuationTimeRemaining <= 0f)
        {
            OpenExit();
        }

        UpdateTimerText();
    }

    public void PlayerEscaped()
    {
        if (roundEnded || !exitOpened)
        {
            return;
        }

        roundEnded = true;
        Time.timeScale = 0f;

        ShowEndPanel(victoryPanel);
    }

    public void RestartGame()
    {
        string currentScene =
            SceneManager.GetActiveScene().name;

        if (PixelTransition.Instance != null)
        {
            PixelTransition.Instance.TransitionToScene(
                currentScene
            );

            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(currentScene);
    }

    private IEnumerator BeginRoundCountdown()
    {
        startCountdownText.gameObject.SetActive(true);

        for (int number = startingCountdown;
             number > 0;
             number--)
        {
            startCountdownText.text = number.ToString();

            yield return new WaitForSecondsRealtime(1f);
        }

        startCountdownText.text = "GO!";

        roundStarted = true;
        Time.timeScale = 1f;
        SetPlayerControls(true);

        yield return new WaitForSecondsRealtime(
            goDisplayDuration
        );

        startCountdownText.gameObject.SetActive(false);
    }

    private void SetPlayerControls(bool controlsEnabled)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = controlsEnabled;
        }

        if (timeSwapController != null)
        {
            timeSwapController.enabled = controlsEnabled;
        }
    }

    private void OpenExit()
    {
        exitOpened = true;
        exitZone.SetOpen(true);
        evacuationTimerText.text = "EXIT OPEN";
    }

    private void HandlePlayerExpired(
    TimeHolder expiredTimeHolder)
    {
        if (roundEnded)
        {
            return;
        }

        roundEnded = true;
        Time.timeScale = 0f;

        ShowEndPanel(deathPanel);
    }

    private void UpdateTimerText()
    {
        if (exitOpened)
        {
            evacuationTimerText.text = "EXIT OPEN";
            return;
        }

        int displayedTime =
            Mathf.CeilToInt(evacuationTimeRemaining);

        evacuationTimerText.text =
            $"EVACUATION: {displayedTime}";
    }

    private void OnDestroy()
    {
        if (playerTime != null)
        {
            playerTime.Expired -= HandlePlayerExpired;
        }
    }

    private void ShowEndPanel(GameObject panel)
    {
        if (PixelTransition.Instance != null)
        {
            PixelTransition.Instance.TransitionAction(
                () => panel.SetActive(true)
            );

            return;
        }

        panel.SetActive(true);
    }
}