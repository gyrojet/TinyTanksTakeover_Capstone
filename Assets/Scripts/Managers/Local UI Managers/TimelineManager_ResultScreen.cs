using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager_ResultScreen : MonoBehaviour
{
    [SerializeField] PlayableDirector results_START;
    [SerializeField] PlayableDirector results_END;

    public static TimelineManager_ResultScreen instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void StartEntranceTimeline()
    {
        print("Starting Entrance Timeline");
        results_START.Play();
    }

    public void StartEndTimeline()
    {
        print("Starting End Timeline");
        results_END.Play();
    }
}
