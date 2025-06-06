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

        levelsFinished.text = livesManager.LevelsWon.ToString();

        int medalIndex = 0;

        int levelsDone = livesManager.LevelsWon;
        string performance = string.Empty;

        if (levelsDone == 0)
        {
            performance = "ABYSSMAL!";
            medal.sprite = medalBAD;

            medalIndex = 1;
        }
        else if (levelsDone >= 1 && levelsDone < 4)
        {
            performance = "MEDIOCRE!";
            medal.sprite = medalOK;

            medalIndex = 2;
        }
        else if (levelsDone >= 4 && levelsDone <= 6)
        {
            performance = "DECENT!";
            medal.sprite = medalGOOD;

            medalIndex = 3;
        }
        else if (levelsDone >= 7 && levelsDone <= 11)
        {
            performance = "GREAT!";
            medal.sprite = medalGOOD;

            medalIndex = 3;
        }
        else if (levelsDone >= 12)
        {
            performance = "AMAZING!";
            medal.sprite = medalGREAT;

            medalIndex = 4;
        }


        if (medalIndex > PlayerPrefs.GetInt("MedalScore"))
        {
            PlayerPrefs.SetInt("MedalScore", medalIndex);
            PlayerPrefs.Save();
        }

        performanceReview.text = performance;
    }

    private void ReturnToTitle_Event()
    {
        sfxManager.PlaySFX(buttonPress, gameObject.transform, 1f);
        timelineManager.StartEndTimeline();
    }
}
