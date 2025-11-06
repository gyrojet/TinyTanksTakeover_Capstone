using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;

    [SerializeField] AudioSource sfxObj;
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
    
    public void PlaySFX(AudioClip clip, Transform objSpawnPoint, float volume)
    {
        // Assign audio source and get data
        AudioSource audioSource = Instantiate(sfxObj, objSpawnPoint.position, Quaternion.identity);

        audioSource.clip = clip;

       // audioSource.volume = volume;

        float clipLength = audioSource.clip.length;

        // Play sound
        audioSource.Play();

        // Destroy sfx setter
        Destroy(audioSource.gameObject, clipLength);
    }
}
