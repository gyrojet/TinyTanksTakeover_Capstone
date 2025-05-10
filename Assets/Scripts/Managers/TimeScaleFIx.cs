using UnityEngine;

public class TimeScaleFIx : MonoBehaviour
{
    private void Awake()
    {
        if (Time.timeScale != 1.0f)
            Time.timeScale = 1.0f;
    }
}
