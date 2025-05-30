using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [Header("Pause Button")]
    public KeyCode pauseButton;

    public static LevelUIManager Instance;

    public bool isRoundStarted = false;

    LevelTransmitionManager levelTransmitionManager;
    RoundManager roundManager;

    LivesManager livesManager;

    private const float TIMESCALE_PAUSED = 0.0f;
    private const float TIMESCALE_RUNNING = 1.0f;

    private const string PAUSESCREEN_ANIMATION_BOOL_OPEN = "Open";
    private const string PAUSESCREEN_ANIMATION_BOOL_CLOSE = "Close";

    [Header("Pause Screen")]
    public GameObject pauseScreen;
    public Animator animator_PauseScreen;

    [Header("Pause Screen Controls")]
    public Button resumeGame;
    public Button endGame;

    [Header("Round Start Timer")]
    [SerializeField] GameObject roundStartTimer;
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Level Display")]
    [SerializeField] TextMeshProUGUI levelDisplayText;
    [SerializeField] TextMeshProUGUI endText;

    [Header("Tanks Remaining Display")]
    public TextMeshProUGUI numberOfTanksRemaining;

    MusicManager musicManager;
    SfxManager sfxManager;

    [SerializeField] AudioClip pauseClip;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (musicManager == null)
            musicManager = MusicManager.instance;

        if (sfxManager == null)
            sfxManager = SfxManager.instance;

        if (livesManager == null)
            livesManager = LivesManager.instance;

        pauseClip = Resources.Load<AudioClip>("Sounds/SFX/UI/PauseSound");

        levelTransmitionManager = LevelTransmitionManager.instance;
        roundManager = RoundManager.instance;

        resumeGame.onClick.AddListener((TogglePause));

        endGame.onClick.AddListener(levelTransmitionManager.ReturnToTitle);

        endGame.onClick.AddListener(() => {
            musicManager.StopMusic();
            musicManager.SetMusicVolume(musicManager.MaxVolume);
            livesManager.SetLevel(1);
        });
    }

    private void Update()
    {
        if (isRoundStarted)
        {
            if (Input.GetKeyDown(pauseButton))
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        sfxManager.PlaySFX(pauseClip, gameObject.transform, 1f);

        if (pauseScreen.activeSelf == false)
        {
            Time.timeScale = TIMESCALE_PAUSED;
            pauseScreen.SetActive(true);

            musicManager.SetMusicVolume(musicManager.MinVolume);

            roundManager.ToggleBehavioursOfAllTanks(false);
        }
        else if (pauseScreen.activeSelf == true)
        {
            Time.timeScale = TIMESCALE_RUNNING;
            pauseScreen.SetActive(false);

            musicManager.SetMusicVolume(musicManager.MaxVolume);

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

    public void UpdateLevelDisplay(int level)
    { 
        levelDisplayText.text = level.ToString();
    }

    public void UpdateEndLevelText(string text)
    { 
        endText.text = text;
    }
}
