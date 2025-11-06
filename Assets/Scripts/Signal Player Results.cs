using UnityEngine;

public class SignalPlayerResults : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;
    LevelTransmitionManager levelManager;

    LivesManager livesManager;
    private void Awake()
    {
        if (musicManager == null)
            musicManager = MusicManager.instance;

        if (levelManager == null)
            levelManager = LevelTransmitionManager.instance;

        if (livesManager == null)
            livesManager = LivesManager.instance;
    }

    public void PlayResultsMusic()
    {
        musicManager.PlayResultsMusic();
    }

    public void StopMusic()
    {
        musicManager.StopMusic();
    }

    public void LoadTitleScreen()
    {
        levelManager.ReturnToTitle();
    }

    public void ResetStats()
    { 
        livesManager.ResetAll();
    }
}
