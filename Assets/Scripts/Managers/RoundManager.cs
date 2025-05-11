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

    [Header("Enemies & Player")]
    [SerializeField] Player player = null;
    [SerializeField] List<GameObject> activeEnemies = null;

    public int numOfActiveEnemies;

    [Header("Identification")]
    [SerializeField] string enemyTag;

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

        GetActivePlayer();
        GetActiveEnemies();

        levelUIManager.UpdateTankCount(numOfActiveEnemies);
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
            StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(3f);

        levelTransmitionManager.LoadSceneWithTransition(
                levelTransmitionManager.GetCurrentSceneIndex());
    }

    public void ToggleBehavioursOfAllTanks(bool value)
    {
        player.playerInputHandler.canPlayerMove = value;

        foreach (GameObject tank in activeEnemies)
        {
            tank.GetComponent<EnemyBehaviourHandler>().isEnabled = value;
        }
    }
}
