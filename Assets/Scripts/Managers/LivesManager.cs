using Pathfinding.Serialization;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    [SerializeField] int lives;

    public int Lives {  get { return lives; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
}
