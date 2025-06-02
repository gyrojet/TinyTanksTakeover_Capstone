using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TitleScreen_UI_Manager : MonoBehaviour
{
    [SerializeField] SfxManager sfxManager;

    [Header("Button")]
    public Button startGame_TEST;
    public bool isIntroOver;

    [SerializeField] AudioClip sfxStart;

    PlayableDirector timelineManager;

    [SerializeField] PlayableDirector manager_START_GAME;
    [SerializeField] PlayableDirector manager_HowToPlay_OPEN;
    //[SerializeField] PlayableDirector manager_HowToPlay_END;

    [Header("Timeline Assets")]
    [SerializeField] TimelineAsset titleScreen_START;
    //[SerializeField] TimelineAsset titleScreen_END;

    private void Start()
    {
        if (sfxManager == null)
            sfxManager = SfxManager.instance;

        isIntroOver = false;
    }

    public void ToggleIsIntroOver()
    {
        isIntroOver = !isIntroOver;
    }

    public void StartExitTimeline()
    {
        sfxManager.PlaySFX(sfxStart, gameObject.transform, 1f);

        print("Starting Exit Timeline!");
        manager_START_GAME.Play();
    }

    public void StartExpandPanel()
    {
        sfxManager.PlaySFX(sfxStart, gameObject.transform, 1f);

        manager_HowToPlay_OPEN.Play();
    }
}
