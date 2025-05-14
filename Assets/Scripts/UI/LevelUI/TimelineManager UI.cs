using UnityEngine;
using UnityEngine.Playables;

public class TimelineManagerUI : MonoBehaviour
{
    public static TimelineManagerUI instance;

    PlayableDirector playableDirector;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        if (playableDirector == null) 
            playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayTimeline()
    {
        playableDirector.Play();
    }
}
