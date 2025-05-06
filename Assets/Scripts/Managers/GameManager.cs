using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    public float delay;

    private void Awake()
    {
        // Creates new instnce or removes old instance if needed
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator ReloadWithDelay()
    {
        yield return new WaitForSeconds(delay);
        ReloadCurrentLevel();
    }
}
