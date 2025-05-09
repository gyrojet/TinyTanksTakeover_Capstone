using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransmitionManager : MonoBehaviour
{
    public static LevelTransmitionManager instance;

    public Animator animator;

    public float transitionDelay = 1.5f;

    const string ANIMATION_TRIGGER_FADEOUT = "TransitionTrigger_FadeToBlack";
    const string ANIMATION_TRIGGER_FADEIN = "TransitionTrigger_FadeIntoNewScene";

    const string ANIMATION_TRIGGER_ENTERIDLE = "TransitionTrigger_EnterIdle";
    const string ANIMATION_TRIGGER_EXITIDLE = "TransitionTrigger_ExitIdle";

    // Add list of scenes!

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Scene Transition Tests
        if (Input.GetKeyDown(KeyCode.Space))
            LoadSceneWithTransition(1);
    }

    public void LoadSceneWithTransition(int sceneIndex)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex != 0)
            animator.SetTrigger(ANIMATION_TRIGGER_FADEOUT);

        StartCoroutine(DelayLoadScene(sceneIndex));
    }

    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private IEnumerator DelayLoadScene(int sceneIndex)
    {
        //yield return new WaitForSecondsRealtime(transitionDelay);

        SceneManager.LoadScene(sceneIndex);

        yield return new WaitForSecondsRealtime(transitionDelay);

        animator.SetTrigger(ANIMATION_TRIGGER_FADEIN);
    }
}
