using UnityEngine;
using Unity;
using System;

public class TimeScaleFIx : MonoBehaviour
{
    private void Awake()
    {
        if (Time.timeScale != 1.0f)
            Time.timeScale = 1.0f;
    }
}
