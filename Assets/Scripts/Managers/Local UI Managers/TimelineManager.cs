using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] UnityEvent<int> callNewLevel = new UnityEvent<int>();

    SignalReceiver signalReceiver;
    [SerializeField] SignalAsset loadLevelSignal;
    LevelTransmitionManager levelTransmitionManager;

    private void Awake()
    {
        signalReceiver = GetComponent<SignalReceiver>();
    }

    void Start()
    {
        if (levelTransmitionManager == null)
            levelTransmitionManager = LevelTransmitionManager.instance;

        
    }

    
}
