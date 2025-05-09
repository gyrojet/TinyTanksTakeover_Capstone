using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    GameManager gameManager;
    LevelTransmitionManager levelTransmitionManager;

    [Header("Enemies & Player")]
    [SerializeField] Player player = null;
    [SerializeField] GameObject[] activeEnemies = null;

    int numOfActiveEnemies;

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
        if (gameManager == null)
            gameManager = GameManager.gameManagerInstance;

        if (levelTransmitionManager == null)
            levelTransmitionManager = LevelTransmitionManager.instance;

        GetActivePlayer();
        GetActiveEnemies();

    }

    private void GetActivePlayer()
    {
        player = Player.playerInstance;
    }

    private void GetActiveEnemies()
    {
        activeEnemies = GameObject.FindGameObjectsWithTag(enemyTag);
        numOfActiveEnemies = activeEnemies.Length;
    }

    public void UpdateCount()
    {
        numOfActiveEnemies--;

        if (numOfActiveEnemies <= 0)
            LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        levelTransmitionManager.LoadSceneWithTransition(
                levelTransmitionManager.GetCurrentSceneIndex());
    }
}
