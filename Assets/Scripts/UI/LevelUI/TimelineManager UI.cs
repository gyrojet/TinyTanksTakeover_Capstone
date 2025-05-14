using UnityEngine;
using UnityEngine.Playables;

public class TimelineManagerUI : MonoBehaviour
{
    public static TimelineManagerUI instance;

    PlayableDirector playableDirector;

    [SerializeField] PlayableAsset roundStart;
    [SerializeField] PlayableAsset roundEnd;

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

    public void PlayRoundStartTimeline()
    {
        playableDirector.playableAsset = roundStart;
        playableDirector.Play();
    }

    public void PlayRoundEndTimeline()
    {
        playableDirector.playableAsset = roundEnd;
        playableDirector.Play();
    }
}
