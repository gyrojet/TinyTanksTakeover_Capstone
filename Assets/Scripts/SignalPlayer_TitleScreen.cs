using UnityEngine;

public class SignalPlayer_TitleScreen : MonoBehaviour
{
    MusicManager musicManager;

    public int sceneToCall = 1;
    [SerializeField] LevelTransmitionManager transmitionManager;

    void Start()
    {
        if (transmitionManager == null)
            transmitionManager = LevelTransmitionManager.instance;

        if (musicManager == null)
        {
            try
            {
                musicManager = MusicManager.instance;
            }
            catch (UnityException e) 
            {
                print(e.Message);
            }
        }
    }

    public void LoadLevel_Inbetween()
    {
        transmitionManager.LoadSceneWithTransition(sceneToCall);
    }

    public void PlayMusicFromManager()
    {
        musicManager.PlayMainMenuMusic();
    }

    public void StopMusicFromManager()
    {
        musicManager.StopMusic();
    }
}
