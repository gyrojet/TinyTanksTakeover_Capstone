using UnityEngine;
using UnityEngine.UI;

public class MedalScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] Image medalToDisplay;

    [SerializeField] Sprite[] medalSprites;

    void Start()
    {
        GetMedalToDisplay();
    }

    private void GetMedalToDisplay()
    {
        if (PlayerPrefs.HasKey("MedalScore"))
        {
            try
            {
                int medalIndex = PlayerPrefs.GetInt("MedalScore");

                medalToDisplay.sprite = medalSprites[medalIndex];
            }
            catch (UnityException e)
            {
                print(e.Message);
            }
        }
        else
        {
            PlayerPrefs.SetInt("MedalScore", 0);
        }

        PlayerPrefs.Save();
    }
}
