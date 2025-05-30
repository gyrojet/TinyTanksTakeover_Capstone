using UnityEngine;

public class SignalPlayerResults : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;
    LevelTransmitionManager levelManager;
    private void Awake()
    {
        if (musicManager == null)
            musicManager = MusicManager.instance;

        if (levelManager == null)
            levelManager = LevelTransmitionManager.instance;
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
}
