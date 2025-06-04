using UnityEngine;
using UnityEngine.UI;

public class SignalPlayer_TitleScreen : MonoBehaviour
{
    MusicManager musicManager;
    LivesManager livesManager;

    public int sceneToCall = 1;
    [SerializeField] LevelTransmitionManager transmitionManager;

    [SerializeField] Button start;
    [SerializeField] Button quit;

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

        if (livesManager == null)
            livesManager = LivesManager.instance;
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

    public void ToggleButton()
    {
        start.interactable = !start.interactable;
        quit.interactable = !quit.interactable;
    }

    public void ResetStats()
    {
        livesManager.ResetAll();
    }
}
