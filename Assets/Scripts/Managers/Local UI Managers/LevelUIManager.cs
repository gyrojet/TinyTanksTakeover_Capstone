using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    LevelTransmitionManager levelTransmitionManager;
    RoundManager roundManager;

    private const float TIMESCALE_PAUSED = 0.0f;
    private const float TIMESCALE_RUNNING = 1.0f;

    private const string PAUSESCREEN_ANIMATION_BOOL_OPEN = "Open";
    private const string PAUSESCREEN_ANIMATION_BOOL_CLOSE = "Close";

    [Header("Pause Screen")]
    public GameObject pauseScreen;
    public Animator animator_PauseScreen;

    [Header("Pause Screen Controls")]
    #region PauseScreenControls

    [Header("Round Start Timer")]
    [SerializeField] GameObject roundStartTimer;
    [SerializeField] TextMeshProUGUI timerText;

    public Button resumeGame;
    public Button endGame;

    #endregion

    public TextMeshProUGUI numberOfTanksRemaining;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        levelTransmitionManager = LevelTransmitionManager.instance;
        roundManager = RoundManager.instance;

        resumeGame.onClick.AddListener((TogglePause));
        endGame.onClick.AddListener(levelTransmitionManager.ReturnToTitle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (pauseScreen.activeSelf == false)
        {
            Time.timeScale = TIMESCALE_PAUSED;
            pauseScreen.SetActive(true);

            roundManager.ToggleBehavioursOfAllTanks(false);
        }
        else if (pauseScreen.activeSelf == true)
        {
            Time.timeScale = TIMESCALE_RUNNING;
            pauseScreen.SetActive(false);

            roundManager.ToggleBehavioursOfAllTanks(true);
        }
    }

    public void UpdateTankCount(int numberRemaining)
    {
        numberOfTanksRemaining.text = numberRemaining.ToString();
    }

    public void UpdateRoundStartTimer(int num)
    {
        timerText.text = num.ToString();
    }

    // REPLACE WITH TIMELINE ANIMATION
    public void ToggleRoundStartTimer()
    {
        roundStartTimer.SetActive(!roundStartTimer.activeSelf);
    }
}
