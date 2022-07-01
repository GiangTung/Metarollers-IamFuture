using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public static event Action SpeedIncrease;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }


    public void SpeedButton()
    {
        if (PlayerControl.instance.speedIncreasedTimes == PlayerControl.instance.speedIncreaseLimit)
            return;
        PlayerControl.instance.speedIncreasedTimes++;
        SpeedIncrease?.Invoke();
    }

}
