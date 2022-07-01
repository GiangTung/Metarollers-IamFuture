using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void VibrationWithDelay(long milliseconds, float timer) // #param1 Duration, #param2 Delay
    {
        StartCoroutine(VibrateDelay(milliseconds, timer));
    }

    IEnumerator VibrateDelay(long milliseconds, float timer)
    {
        yield return new WaitForSeconds(timer);
        Vibration.Vibrate(milliseconds);
    }

}
