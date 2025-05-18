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

    [Header("Enemies & Player")]
    [SerializeField] Player player = null;
    [SerializeField] List<GameObject> activeEnemies = null;

    public int numOfActiveEnemies;

    [Header("Round Start Sequence")]

    public int roundStartCountdown;

    [Header("Identification")]
    [SerializeField] string enemyTag;
    [SerializeField] string bulletTag;
    [SerializeField] string mineTag;

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
        //if (gameManager == null)
        //    gameManager = GameManager.gameManagerInstance;

        if (levelTransmitionManager == null)
            levelTransmitionManager = LevelTransmitionManager.instance;

        if (levelUIManager == null)
            levelUIManager = LevelUIManager.Instance;

        if (timelineManagerUI == null)
            timelineManagerUI = TimelineManagerUI.instance;

        levelUIManager.isRoundStarted = false;

        GetActivePlayer();
        GetActiveEnemies();

        ToggleBehavioursOfAllTanks(false);

        levelUIManager.UpdateTankCount(numOfActiveEnemies);

        // Quick fix to show to jason...
        levelUIManager.UpdateLevelDisplay(1);

        StartCoroutine(RoundStartSequence());
    }

    private void GetActivePlayer()
    {
        player = Player.playerInstance;
    }

    private void GetActiveEnemies()
    {
        activeEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(enemyTag));
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
        DestroyAllMunitions();
        ToggleBehavioursOfAllTanks(false);
        StopPlayerMovment();

        if (!isPlayerDead)
            timelineManagerUI.PlayRoundEndTimeline();

        // If player is dead, reload level
        // If not load new one
        // Bool has no use as of yet
        
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(3f);

        // Change later...
        levelTransmitionManager.LoadSceneWithTransition(
                levelTransmitionManager.GetCurrentSceneIndex());
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
    }

    public void ToggleBehavioursOfAllTanks(bool value)
    {
        player.playerInputHandler.canPlayerMove = value;

        foreach (GameObject tank in activeEnemies)
        {
            EnemyBehaviourHandler enemy = tank.GetComponent<EnemyBehaviourHandler>();

            enemy.isEnabled = value;
            
            if (enemy.canMove)
                enemy.pathfinder.canMove = value;
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

            yield return new WaitForSecondsRealtime(1f);

            countdown--;
        }

        //levelUIManager.ToggleRoundStartTimer();

        timelineManagerUI.PlayRoundStartTimeline();

        ToggleBehavioursOfAllTanks(true);

        levelUIManager.isRoundStarted = true;
    }
}
