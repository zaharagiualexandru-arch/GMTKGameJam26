using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PixelTransition : MonoBehaviour
{
    public static PixelTransition Instance { get; private set; }

    [SerializeField] private RectTransform blockContainer;
    [SerializeField] private int columns = 16;
    [SerializeField] private int rows = 9;
    [SerializeField] private float stepDelay = 0.04f;
    [SerializeField] private float coveredPause = 0.15f;
    [SerializeField] private Color blockColour = Color.black;

    private Canvas transitionCanvas;
    private GameObject[,] blocks;
    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        transitionCanvas = GetComponent<Canvas>();

        CreateBlocks();
        transitionCanvas.enabled = false;
    }

    public void TransitionToScene(string sceneName)
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(SceneTransitionRoutine(sceneName));
    }

    public void TransitionAction(Action coveredAction)
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(ActionTransitionRoutine(
            coveredAction
        ));
    }

    private void CreateBlocks()
    {
        blocks = new GameObject[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0;
                 column < columns;
                 column++)
            {
                GameObject block = new GameObject(
                    $"Block_{row}_{column}",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Image)
                );

                block.transform.SetParent(
                    blockContainer,
                    false
                );

                RectTransform rect =
                    block.GetComponent<RectTransform>();

                float minimumX =
                    (float)column / columns;

                float maximumX =
                    (float)(column + 1) / columns;

                float maximumY =
                    1f - (float)row / rows;

                float minimumY =
                    1f - (float)(row + 1) / rows;

                rect.anchorMin =
                    new Vector2(minimumX, minimumY);

                rect.anchorMax =
                    new Vector2(maximumX, maximumY);

                rect.offsetMin =
                    new Vector2(-1f, -1f);

                rect.offsetMax =
                    new Vector2(1f, 1f);

                Image image = block.GetComponent<Image>();
                image.color = blockColour;
                image.raycastTarget = false;

                block.SetActive(false);
                blocks[row, column] = block;
            }
        }
    }

    private IEnumerator SceneTransitionRoutine(
        string sceneName)
    {
        isTransitioning = true;
        transitionCanvas.enabled = true;

        yield return SetBlocksVisible(true);

        yield return new WaitForSecondsRealtime(
            coveredPause
        );

        Time.timeScale = 1f;

        AsyncOperation loadOperation =
            SceneManager.LoadSceneAsync(sceneName);

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(
            coveredPause
        );

        yield return SetBlocksVisible(false);

        transitionCanvas.enabled = false;
        isTransitioning = false;
    }

    private IEnumerator ActionTransitionRoutine(
        Action coveredAction)
    {
        isTransitioning = true;
        transitionCanvas.enabled = true;

        yield return SetBlocksVisible(true);

        yield return new WaitForSecondsRealtime(
            coveredPause
        );

        coveredAction?.Invoke();

        yield return new WaitForSecondsRealtime(
            coveredPause
        );

        yield return SetBlocksVisible(false);

        transitionCanvas.enabled = false;
        isTransitioning = false;
    }

    private IEnumerator SetBlocksVisible(bool visible)
    {
        int diagonalCount = rows + columns - 1;

        for (int diagonal = 0;
             diagonal < diagonalCount;
             diagonal++)
        {
            for (int row = 0; row < rows; row++)
            {
                int column = diagonal - row;

                if (column < 0 || column >= columns)
                {
                    continue;
                }

                blocks[row, column].SetActive(visible);
            }

            yield return new WaitForSecondsRealtime(
                stepDelay
            );
        }
    }
}