using UnityEngine;

public class SignalPlayerResults : MonoBehaviour
{
    [SerializeField] MusicManager musicManager;

    private void Awake()
    {
        if (musicManager == null)
            musicManager = MusicManager.instance;
    }

    public void PlayResultsMusic()
    {
        musicManager.PlayResultsMusic();
    }

    public void StopMusic()
    {
        musicManager.StopMusic();
    }
}
