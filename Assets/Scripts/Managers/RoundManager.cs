using Pathfinding;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    //GameManager gameManager;
    LevelTransmitionManager levelTransmitionManager;
    LevelUIManager levelUIManager;
    TimelineManagerUI timelineManagerUI;
    LivesManager livesManager;

    MusicManager musicManager;
    SfxManager sfxManager;

    [Header("Enemies & Player")]
    [SerializeField] Player player = null;
    [SerializeField] List<GameObject> activeEnemies = null;
    List<GameObject> enemiesBackup = null;

    public int numOfActiveEnemies;

    [Header("Round Start Sequence")]

    public int roundStartCountdown;
    public AudioClip countdownSfx;
    public AudioClip endRound;
    public AudioClip startRound_Doot;
    public AudioClip failLevel;

    [Header("Identification")]
    [SerializeField] string enemyTag;
    [SerializeField] string bulletTag;
    [SerializeField] string mineTag;

    [Header("ObjectPooling")]
    [SerializeField] TankTrack tankTrackPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create track pool
        //ObjectPool.SetupItemPool(tankTrackPrefab, 250, "TankTracks");

        if (levelTransmitionManager == null)
            levelTransmitionManager = LevelTransmitionManager.instance;

        if (levelUIManager == null)
            levelUIManager = LevelUIManager.Instance;

        if (timelineManagerUI == null)
            timelineManagerUI = TimelineManagerUI.instance;

        if (musicManager == null)
            musicManager = MusicManager.instance;

        if (sfxManager == null)
            sfxManager = SfxManager.instance;

        if (livesManager == null)
            livesManager = LivesManager.instance;

        levelUIManager.isRoundStarted = false;

        GetActivePlayer();
        GetActiveEnemies();

        ToggleBehavioursOfAllTanks(false);

        levelUIManager.UpdateTankCount(numOfActiveEnemies);

        // Quick fix to show to jason...
        levelUIManager.UpdateLevelDisplay(livesManager.CurrentLevel);
        levelUIManager.UpdateLivesText(livesManager.Lives);

        StartCoroutine(RoundStartSequence());
    }

    private void GetActivePlayer()
    {
        player = Player.playerInstance;
    }

    private void GetActiveEnemies()
    {
        activeEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(enemyTag));

        enemiesBackup = new List<GameObject>(activeEnemies);

        numOfActiveEnemies = activeEnemies.Count;
    }

    public void UpdateCount(GameObject tankToRemove)
    {
        numOfActiveEnemies--;

        activeEnemies.Remove(tankToRemove);

        levelUIManager.UpdateTankCount(numOfActiveEnemies);

        if (numOfActiveEnemies <= 0)
        {
            EndingSequence(false);
        }
    }

    public void EndingSequence(bool isPlayerDead)
    {
        musicManager.StopMusic();

        DestroyAllMunitions();
        ToggleBehavioursOfAllTanks(false);
        StopPlayerMovment();

        if (!isPlayerDead)
        {
            if (livesManager.CurrentLevel % 3 == 0)
            {
                livesManager.IncreaseLives();
                levelUIManager.UpdateFace(":^D");
                levelUIManager.UpdateEndLevelText("+1 LIFE!");
            }
            else
            {
                levelUIManager.UpdateFace(":^)");
                levelUIManager.UpdateEndLevelText("AWESOME!");
            }

            sfxManager.PlaySFX(endRound, gameObject.transform, 1f);
        }
        else
        {
            sfxManager.PlaySFX(failLevel, gameObject.transform, 1f);
        }

        if (!isPlayerDead)
        {
            if (livesManager.CurrentLevel != 10)
                livesManager.IncrementCurrentLevel();

            StartCoroutine(LoadNextLevel());
        }
        else
        {
            levelUIManager.UpdateFace(":^(");
            levelUIManager.UpdateEndLevelText("OOPS!");

            if (livesManager.Lives <= 0)
            {
                StartCoroutine(GameOver());
            }
            else
            {
                StartCoroutine(ReloadLevel());
            }
        }

        timelineManagerUI.PlayRoundEndTimeline();
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(3f);

        try
        {
            //livesManager.SetNumberOfLives(3);
            levelTransmitionManager.LoadResultsScreen();
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
    }

    private IEnumerator ReloadLevel()
    {
        yield return new WaitForSecondsRealtime(3f);

        try
        {
            levelTransmitionManager.LoadSceneWithTransition(
                    levelTransmitionManager.GetCurrentSceneIndex());
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(3f);

        try
        {
            levelTransmitionManager.LoadSceneWithTransition(
                    levelTransmitionManager.GetCurrentSceneIndex() + 1);
        }
        catch (UnityException e) 
        {
            Debug.Log(e.Message);
        }
    }

    public void DestroyAllMunitions()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag(bulletTag);
        GameObject[] mines = GameObject.FindGameObjectsWithTag(mineTag);

        foreach (GameObject bullet in bullets)
        {
            Bullet b = bullet.GetComponent<Bullet>();
            b.DestroySelf(true);
        }

        foreach (GameObject mine in mines)
        {
            Mine m = mine.GetComponent<Mine>();
            m.FakeExplode();
        }

        foreach (GameObject enemy in enemiesBackup)
        {
            Enemy e = enemy.GetComponent<Enemy>();

            e.StopAllCoroutines();
        }
    }

    public void ToggleBehavioursOfAllTanks(bool value)
    {
        player.playerInputHandler.canPlayerMove = value;

        if (value)
            player.engineNoise.Play();
        else if (!value)
            player.engineNoise.Stop();

            foreach (GameObject tank in activeEnemies)
            {
                EnemyBehaviourHandler enemy = tank.GetComponent<EnemyBehaviourHandler>();

                enemy.isEnabled = value;

                if (enemy.canMove)
                    enemy.pathfinder.canMove = value;

                if (enemy.canMakeEngineNoises)
                {
                    if (value)
                    {
                        enemy.audioSource.Play();
                    }
                    else
                    {
                        enemy.audioSource.Stop();
                    }
                }
            }
    }

    private void StopPlayerMovment()
    {
        player.movementVector = Vector2.zero;
    }

    private IEnumerator RoundStartSequence()
    {
        int countdown = roundStartCountdown;

        while (countdown > 0)
        {
            levelUIManager.UpdateRoundStartTimer(countdown);

            sfxManager.PlaySFX(countdownSfx, gameObject.transform, 1f);

            yield return new WaitForSecondsRealtime(1f);

            countdown--;
        }

        //levelUIManager.ToggleRoundStartTimer();

        timelineManagerUI.PlayRoundStartTimeline();

        ToggleBehavioursOfAllTanks(true);

        levelUIManager.isRoundStarted = true;

        sfxManager.PlaySFX(startRound_Doot, gameObject.transform, 1f);

        musicManager.PlayLevelMusic();
    }
}
