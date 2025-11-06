using Pathfinding.Serialization;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    [SerializeField] int lives;
    [SerializeField] int currentLevel;
    [SerializeField] int levelsWon = 0;

    public int Lives {  get { return lives; } }

    public int CurrentLevel {  get { return currentLevel; } }

    public int LevelsWon { get { return levelsWon; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            currentLevel = 1;
        }
        else
            Destroy(gameObject);

        
    }

    public void IncreaseLives()
    {
        lives++;
    }

    public void DecreaseLives()
    {
        lives--;
    }

    public void SetNumberOfLives(int numberToSet)
    {
        lives = numberToSet;
    }

    public void IncrementCurrentLevel()
    {
        currentLevel++;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    public void IncLevelsWon()
    {
        levelsWon++;
    }

    public void ResetAll()
    { 
        levelsWon = 0;
        currentLevel = 1;
        lives = 4;
    }
}
