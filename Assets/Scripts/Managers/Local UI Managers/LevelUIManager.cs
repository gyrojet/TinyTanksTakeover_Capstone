using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
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

    //public Button resumeGame;
    //public Button endGame;

    #endregion

    public TextMeshProUGUI numberOfTanksRemaining;

    private void Start()
    {
        roundManager = RoundManager.instance;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
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
}
