using UnityEngine;

public class SignalPlayer_TitleScreen : MonoBehaviour
{
    public int sceneToCall = 1;
    [SerializeField] LevelTransmitionManager transmitionManager;

    void Start()
    {
        if (transmitionManager == null)
            transmitionManager = LevelTransmitionManager.instance;
    }

    public void LoadLevel_Inbetween()
    {
        transmitionManager.LoadSceneWithTransition(sceneToCall);
    }

    
}
