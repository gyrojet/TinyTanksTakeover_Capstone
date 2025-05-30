using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsUIManager : MonoBehaviour
{
    LivesManager livesManager;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI livesRemaining;
    [SerializeField] TextMeshProUGUI levelsFinished;
    [SerializeField] TextMeshProUGUI performanceReview;

    [SerializeField] Button returnToMenu;


    private void Awake()
    {
        if (livesManager == null)
        {
            livesManager = LivesManager.instance;
        }
    }

    private void Start()
    {
        SetResults();
    }

    private void SetResults()
    {
        livesRemaining.text = livesManager.Lives.ToString();

        levelsFinished.text = livesManager.CurrentLevel.ToString();

        int levelsDone = livesManager.CurrentLevel;
        string performance = string.Empty;

        if (levelsDone == 1)
        {
            performance = "ABYSSMAL!";
        }
        else if (levelsDone >= 2 && levelsDone < 4)
        {
            performance = "MEDIOCRE!";
        }
        else if (levelsDone >= 4 && levelsDone <= 6)
        {
            performance = "DECENT!";
        }
        else if (levelsDone >= 7 && levelsDone <= 9)
        {
            performance = "GREAT!";
        }
        else if (levelsDone >= 10)
        {
            performance = "AMAZING!";
        }

        performanceReview.text = performance;
    }
}
