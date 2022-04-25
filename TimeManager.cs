using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float timeScale;

    void Start()
    {
        Time.timeScale = timeScale;
    }

    void Update()
    {
        Time.timeScale = timeScale;
    }
}
