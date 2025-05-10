using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TitleScreen_UI_Manager : MonoBehaviour
{
    [Header("Button")]
    public Button startGame_TEST;
    public bool isIntroOver;

    PlayableDirector timelineManager;

    [SerializeField] PlayableDirector manager_START_GAME;

    [Header("Timeline Assets")]
    [SerializeField] TimelineAsset titleScreen_START;
    //[SerializeField] TimelineAsset titleScreen_END;

    private void Start()
    {
        isIntroOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Space) && isIntroOver == true)
        //    manager_START_GAME.Play();
    }

    public void ToggleIsIntroOver()
    {
        isIntroOver = !isIntroOver;
    }

    public void StartExitTimeline()
    {
        print("Starting Exit Timeline!");
        manager_START_GAME.Play();
    }
}
