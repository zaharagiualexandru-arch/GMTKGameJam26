using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float evacuationDuration = 60f;
    [SerializeField] private TMP_Text evacuationTimerText;
    [SerializeField] private ExitZone exitZone;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject victoryPanel;

    private TimeHolder playerTime;
    private float evacuationTimeRemaining;
    private bool exitOpened;
    private bool roundEnded;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        playerTime = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponent<TimeHolder>();

        playerTime.Expired += HandlePlayerExpired;

        evacuationTimeRemaining = evacuationDuration;
        deathPanel.SetActive(false);
        victoryPanel.SetActive(false);
        exitZone.SetOpen(false);

        UpdateTimerText();
    }

    private void Update()
    {
        if (roundEnded || exitOpened)
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
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    private void OpenExit()
    {
        exitOpened = true;
        exitZone.SetOpen(true);
        evacuationTimerText.text = "EXIT OPEN";
    }

    private void HandlePlayerExpired(TimeHolder expiredTimeHolder)
    {
        if (roundEnded)
        {
            return;
        }

        roundEnded = true;
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
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
}