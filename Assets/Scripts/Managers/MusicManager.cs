using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    AudioSource musicSource;

    [Header("Music Tracks")]
    [SerializeField] AudioClip mainMenu;
    [SerializeField] AudioClip level_v1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();
    }

    public void PlayMainMenuMusic()
    {
        musicSource.clip = mainMenu;

        musicSource.Play();
    }

    public void PlayLevelMusic()
    {
        musicSource.clip = level_v1;

        musicSource.Play();
    }

    // Will likely be replaced later with audio mixing group
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void StopMusic()
    { 
        musicSource.Stop();
    }
}
