using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FallingCountdownBackground : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset countdownFont;

    [SerializeField] private int maximumTimers = 7;
    [SerializeField] private int initialTimers = 3;

    [SerializeField]
    private Vector2 spawnIntervalRange =
        new Vector2(0.8f, 1.8f);

    [SerializeField]
    private Vector2Int startingNumberRange =
        new Vector2Int(5, 15);

    [SerializeField]
    private Vector2 fallDurationRange =
        new Vector2(6f, 11f);

    [SerializeField]
    private Vector2 fontSizeRange =
        new Vector2(28f, 55f);

    [SerializeField]
    private Vector2 startingRotationRange =
        new Vector2(-30f, 30f);

    [SerializeField]
    private Vector2 spinSpeedRange =
        new Vector2(-20f, 20f);

    [SerializeField]
    private Vector2 horizontalDriftRange =
        new Vector2(-100f, 100f);

    [SerializeField]
    private Vector2 waveAmountRange =
        new Vector2(0f, 60f);

    [SerializeField]
    private Vector2 waveCyclesRange =
        new Vector2(0.5f, 1.5f);

    [SerializeField]
    private Vector2 opacityRange =
        new Vector2(0.08f, 0.16f);

    [SerializeField] private float sidePadding = 80f;
    [SerializeField] private float verticalPadding = 60f;

    [SerializeField]
    private Color normalColour =
        new Color(0.08f, 0.08f, 0.08f);

    [SerializeField]
    private Color accentColour =
        new Color(0.8f, 0.05f, 0.05f);

    [SerializeField] private float accentChance = 0.15f;

    private class CountdownItem
    {
        public RectTransform RectTransform;
        public TMP_Text Text;
        public float Duration;
        public float Elapsed;
        public float StartX;
        public float EndX;
        public float StartY;
        public float EndY;
        public float StartingRotation;
        public float SpinSpeed;
        public float WaveAmount;
        public float WaveCycles;
        public float WavePhase;
        public float Opacity;
        public int StartingNumber;
        public int Format;
        public Color Colour;
    }

    private readonly List<CountdownItem> timers =
        new List<CountdownItem>();

    private RectTransform container;
    private float spawnTimer;

    private void Start()
    {
        container = GetComponent<RectTransform>();

        Canvas.ForceUpdateCanvases();

        int amount = Mathf.Min(initialTimers, maximumTimers);

        for (int i = 0; i < amount; i++)
        {
            CreateTimer(Random.Range(0f, 0.75f));
        }

        ResetSpawnTimer();
    }

    private void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;

        spawnTimer -= deltaTime;

        if (spawnTimer <= 0f &&
            timers.Count < maximumTimers)
        {
            CreateTimer(0f);
            ResetSpawnTimer();
        }

        for (int i = timers.Count - 1; i >= 0; i--)
        {
            CountdownItem timer = timers[i];
            timer.Elapsed += deltaTime;

            float progress =
                Mathf.Clamp01(timer.Elapsed / timer.Duration);

            UpdateTimer(timer, progress);

            if (progress >= 1f)
            {
                Destroy(timer.RectTransform.gameObject);
                timers.RemoveAt(i);
            }
        }
    }

    private void CreateTimer(float initialProgress)
    {
        float width = container.rect.width;
        float height = container.rect.height;

        if (width <= 0f || height <= 0f)
        {
            return;
        }

        GameObject timerObject = new GameObject(
            "FallingCountdown",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI)
        );

        timerObject.transform.SetParent(container, false);

        RectTransform timerRect =
            timerObject.GetComponent<RectTransform>();

        TMP_Text timerText =
            timerObject.GetComponent<TMP_Text>();

        float fontSize =
            Random.Range(fontSizeRange.x, fontSizeRange.y);

        timerRect.sizeDelta = new Vector2(250f, fontSize * 2f);

        timerText.font = countdownFont;
        timerText.fontSize = fontSize;
        timerText.alignment = TextAlignmentOptions.Center;
        timerText.raycastTarget = false;
        timerText.overflowMode = TextOverflowModes.Overflow;

        float horizontalLimit =
            Mathf.Max(0f, width * 0.5f - sidePadding);

        float startX =
            Random.Range(-horizontalLimit, horizontalLimit);

        float endX = Mathf.Clamp(
            startX + Random.Range(
                horizontalDriftRange.x,
                horizontalDriftRange.y
            ),
            -horizontalLimit,
            horizontalLimit
        );

        Color selectedColour =
            Random.value <= accentChance
                ? accentColour
                : normalColour;

        CountdownItem timer = new CountdownItem
        {
            RectTransform = timerRect,
            Text = timerText,
            Duration = Random.Range(
                fallDurationRange.x,
                fallDurationRange.y
            ),
            StartX = startX,
            EndX = endX,
            StartY = height * 0.5f + verticalPadding,
            EndY = -height * 0.5f - verticalPadding,
            StartingRotation = Random.Range(
                startingRotationRange.x,
                startingRotationRange.y
            ),
            SpinSpeed = Random.Range(
                spinSpeedRange.x,
                spinSpeedRange.y
            ),
            WaveAmount = Random.Range(
                waveAmountRange.x,
                waveAmountRange.y
            ),
            WaveCycles = Random.Range(
                waveCyclesRange.x,
                waveCyclesRange.y
            ),
            WavePhase = Random.Range(0f, Mathf.PI * 2f),
            Opacity = Random.Range(
                opacityRange.x,
                opacityRange.y
            ),
            StartingNumber = Random.Range(
                startingNumberRange.x,
                startingNumberRange.y + 1
            ),
            Format = Random.Range(0, 4),
            Colour = selectedColour
        };

        timer.Elapsed = timer.Duration * initialProgress;

        timers.Add(timer);
        UpdateTimer(timer, initialProgress);
    }

    private void UpdateTimer(
        CountdownItem timer,
        float progress)
    {
        float numberProgress =
            Mathf.Clamp01(progress / 0.85f);

        int displayedNumber = Mathf.CeilToInt(
            Mathf.Lerp(
                timer.StartingNumber,
                0f,
                numberProgress
            )
        );

        timer.Text.text = FormatNumber(
            displayedNumber,
            timer.Format
        );

        float x = Mathf.Lerp(
            timer.StartX,
            timer.EndX,
            progress
        );

        x += Mathf.Sin(
            progress *
            Mathf.PI *
            2f *
            timer.WaveCycles +
            timer.WavePhase
        ) * timer.WaveAmount;

        float y = Mathf.Lerp(
            timer.StartY,
            timer.EndY,
            progress
        );

        timer.RectTransform.anchoredPosition =
            new Vector2(x, y);

        timer.RectTransform.localRotation =
            Quaternion.Euler(
                0f,
                0f,
                timer.StartingRotation +
                timer.SpinSpeed * timer.Elapsed
            );

        float fade = 1f;

        if (progress >= 0.85f)
        {
            fade = 1f - Mathf.InverseLerp(
                0.85f,
                1f,
                progress
            );
        }

        Color colour = timer.Colour;
        colour.a = timer.Opacity * fade;
        timer.Text.color = colour;
    }

    private string FormatNumber(int number, int format)
    {
        switch (format)
        {
            case 1:
                return number.ToString("00");

            case 2:
                return $"00:{number:00}";

            case 3:
                return $"[{number}]";

            default:
                return number.ToString();
        }
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(
            spawnIntervalRange.x,
            spawnIntervalRange.y
        );
    }
}