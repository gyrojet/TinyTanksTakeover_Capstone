using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsUIManager : MonoBehaviour
{
    SfxManager sfxManager;
    LivesManager livesManager;
    TimelineManager_ResultScreen timelineManager;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI livesRemaining;
    [SerializeField] TextMeshProUGUI levelsFinished;
    [SerializeField] TextMeshProUGUI performanceReview;

    [SerializeField] Image medal;

    [SerializeField] Button returnToMenu;

    [Header("Medals")]
    [SerializeField] Sprite medalBAD;
    [SerializeField] Sprite medalOK;
    [SerializeField] Sprite medalGOOD;
    [SerializeField] Sprite medalGREAT;

    [Header("SFX")]
    [SerializeField] AudioClip buttonPress;

    private void Awake()
    {
        if (livesManager == null)
        {
            livesManager = LivesManager.instance;
        }
    }

    private void Start()
    {
        if (livesManager == null)
        {
            livesManager = LivesManager.instance;
        }

        if (timelineManager == null)
        {
            timelineManager = TimelineManager_ResultScreen.instance;
        }

        if (sfxManager == null)
            sfxManager = SfxManager.instance;

        returnToMenu.onClick.AddListener(ReturnToTitle_Event);

        SetResults();
        timelineManager.StartEntranceTimeline();
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
            medal.sprite = medalBAD;
        }
        else if (levelsDone >= 2 && levelsDone < 4)
        {
            performance = "MEDIOCRE!";
            medal.sprite = medalOK;
        }
        else if (levelsDone >= 4 && levelsDone <= 6)
        {
            performance = "DECENT!";
            medal.sprite = medalGOOD;
        }
        else if (levelsDone >= 7 && levelsDone <= 9)
        {
            performance = "GREAT!";
            medal.sprite = medalGOOD;
        }
        else if (levelsDone >= 10)
        {
            performance = "AMAZING!";
            medal.sprite = medalGREAT;
        }

        performanceReview.text = performance;
    }

    private void ReturnToTitle_Event()
    {
        sfxManager.PlaySFX(buttonPress, gameObject.transform, 1f);
        timelineManager.StartEndTimeline();
    }
}
