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

    [SerializeField] private AudioClip[] clockTickSounds;
    [SerializeField] private float countdownTickVolume = 0.8f;
    [SerializeField] private float goTickPitch = 1.3f;

    [SerializeField] private int lowTimeWarningThreshold = 5;
    [SerializeField] private float lowTimeTickVolume = 0.85f;

    [SerializeField]
    private Vector2 lowTimePitchRange =
        new Vector2(0.95f, 1.2f);

    [SerializeField] private AudioSource inGameMusicSource;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    [SerializeField] private float doorOpenVolume = 0.9f;
    [SerializeField] private float winVolume = 1f;
    [SerializeField] private float loseVolume = 1f;

    [SerializeField] private ExitZone exitZone;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject victoryPanel;

    private TimeHolder playerTime;
    private PlayerMovement playerMovement;
    private TimeSwapController timeSwapController;

    private AudioSource clockAudioSource;
    private AudioSource eventAudioSource;

    private float evacuationTimeRemaining;
    private int previousDisplayedPlayerTime;
    private bool roundStarted;
    private bool exitOpened;
    private bool roundEnded;

    private void Awake()
    {
        Time.timeScale = 0f;

        clockAudioSource =
            CreateAudioSource("ClockAudioSource");

        eventAudioSource =
            CreateAudioSource("EventAudioSource");
    }

    private void Start()
    {
        FindGameplayMusicSource();

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        playerTime = player.GetComponent<TimeHolder>();
        playerMovement =
            player.GetComponent<PlayerMovement>();

        timeSwapController =
            player.GetComponent<TimeSwapController>();

        previousDisplayedPlayerTime =
            Mathf.CeilToInt(playerTime.RemainingTime);

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
        if (!roundStarted || roundEnded)
        {
            return;
        }

        UpdateLowTimeWarning();

        if (exitOpened)
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

        clockAudioSource.Stop();
        StopGameplayMusic();
        PlayEventSound(winSound, winVolume);

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
            startCountdownText.text =
                number.ToString();

            int soundIndex =
                startingCountdown - number;

            PlayClockTick(soundIndex, 1f);

            yield return new WaitForSecondsRealtime(1f);
        }

        startCountdownText.text = "GO!";

        PlayClockTick(
            startingCountdown - 1,
            goTickPitch
        );

        roundStarted = true;
        Time.timeScale = 1f;
        SetPlayerControls(true);

        yield return new WaitForSecondsRealtime(
            goDisplayDuration
        );

        startCountdownText.gameObject.SetActive(false);
    }

    private AudioSource CreateAudioSource(
        string objectName)
    {
        GameObject sourceObject =
            new GameObject(objectName);

        sourceObject.transform.SetParent(
            transform,
            false
        );

        AudioSource source =
            sourceObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f;

        return source;
    }

    private void FindGameplayMusicSource()
    {
        if (inGameMusicSource != null)
        {
            return;
        }

        GameObject musicObject =
            GameObject.Find("InGameMusic");

        if (musicObject != null)
        {
            inGameMusicSource =
                musicObject.GetComponent<AudioSource>();
        }
    }

    private void PlayClockTick(
        int soundIndex,
        float pitch)
    {
        if (clockTickSounds == null ||
            clockTickSounds.Length == 0)
        {
            return;
        }

        soundIndex = Mathf.Clamp(
            soundIndex,
            0,
            clockTickSounds.Length - 1
        );

        AudioClip selectedSound =
            clockTickSounds[soundIndex];

        if (selectedSound == null)
        {
            return;
        }

        clockAudioSource.pitch = pitch;

        clockAudioSource.PlayOneShot(
            selectedSound,
            countdownTickVolume
        );
    }

    private void UpdateLowTimeWarning()
    {
        int displayedPlayerTime =
            Mathf.CeilToInt(
                playerTime.RemainingTime
            );

        bool timeDecreased =
            displayedPlayerTime <
            previousDisplayedPlayerTime;

        bool isLowTime =
            displayedPlayerTime > 0 &&
            displayedPlayerTime <=
            lowTimeWarningThreshold;

        if (timeDecreased && isLowTime)
        {
            PlayLowTimeTick(
                displayedPlayerTime
            );
        }

        previousDisplayedPlayerTime =
            displayedPlayerTime;
    }

    private void PlayLowTimeTick(
        int secondsRemaining)
    {
        if (clockTickSounds == null ||
            clockTickSounds.Length == 0)
        {
            return;
        }

        int soundIndex =
            (lowTimeWarningThreshold -
             secondsRemaining) %
            clockTickSounds.Length;

        AudioClip selectedSound =
            clockTickSounds[soundIndex];

        if (selectedSound == null)
        {
            return;
        }

        float urgency = Mathf.InverseLerp(
            lowTimeWarningThreshold,
            1f,
            secondsRemaining
        );

        clockAudioSource.pitch = Mathf.Lerp(
            lowTimePitchRange.x,
            lowTimePitchRange.y,
            urgency
        );

        clockAudioSource.PlayOneShot(
            selectedSound,
            lowTimeTickVolume
        );
    }

    private void PlayEventSound(
        AudioClip sound,
        float volume)
    {
        eventAudioSource.Stop();

        if (sound == null)
        {
            return;
        }

        eventAudioSource.clip = sound;
        eventAudioSource.pitch = 1f;
        eventAudioSource.volume = volume;
        eventAudioSource.Play();
    }

    private void StopGameplayMusic()
    {
        if (inGameMusicSource != null)
        {
            inGameMusicSource.Stop();
        }
    }

    private void SetPlayerControls(
        bool controlsEnabled)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled =
                controlsEnabled;
        }

        if (timeSwapController != null)
        {
            timeSwapController.enabled =
                controlsEnabled;
        }
    }

    private void OpenExit()
    {
        exitOpened = true;
        exitZone.SetOpen(true);
        evacuationTimerText.text = "EXIT OPEN";

        PlayEventSound(
            doorOpenSound,
            doorOpenVolume
        );
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

        clockAudioSource.Stop();
        StopGameplayMusic();
        PlayEventSound(loseSound, loseVolume);

        ShowEndPanel(deathPanel);
    }

    private void UpdateTimerText()
    {
        if (exitOpened)
        {
            evacuationTimerText.text =
                "EXIT OPEN";

            return;
        }

        int displayedTime =
            Mathf.CeilToInt(
                evacuationTimeRemaining
            );

        evacuationTimerText.text =
            $"EVACUATION: {displayedTime}";
    }

    private void OnDestroy()
    {
        if (playerTime != null)
        {
            playerTime.Expired -=
                HandlePlayerExpired;
        }
    }

    private void ShowEndPanel(GameObject panel)
    {
        if (PixelTransition.Instance != null)
        {
            PixelTransition.Instance
                .TransitionAction(
                    () => panel.SetActive(true)
                );

            return;
        }

        panel.SetActive(true);
    }
}