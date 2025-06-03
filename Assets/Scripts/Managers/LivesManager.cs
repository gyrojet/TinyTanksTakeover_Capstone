using Pathfinding.Serialization;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    [SerializeField] int lives;
    [SerializeField] int currentLevel;

    public int Lives {  get { return lives; } }

    public int CurrentLevel {  get { return currentLevel; } }

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
}
