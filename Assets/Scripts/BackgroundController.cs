using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    [SerializeField] SpriteRenderer [] background;
    [SerializeField] Sprite morning, night, afternoon;
    [SerializeField] List<SpriteRenderer> backgroundElements = new List<SpriteRenderer>();
    
    [Header("Speed Button Settings")]
    [SerializeField] float parallaxSpeedIncrease = 0.1f;

    private void OnEnable()
    {
        EventManager.SpeedIncrease += IncreaseParallaxSpeed;
    }

    private void OnDisable()
    {
        EventManager.SpeedIncrease -= IncreaseParallaxSpeed;
    }
    void Start()
    {
        int dayTime = System.DateTime.Now.Hour;
        SetSkyBox(dayTime);
    }

    private void SetSkyBox(int dayTime)
    {
        if (dayTime >= 0 && dayTime <= 14)
        {
           foreach(SpriteRenderer sr in background)
            {
                sr.sprite = morning;
            }
            print("madrugada");
        }

        else if (dayTime > 14 && dayTime < 19)
        {
            foreach (SpriteRenderer sr in background)
            {
                sr.sprite = afternoon;
            }
            print("Tarde");
        }

        else
        {
            foreach (SpriteRenderer sr in background)
            {
                sr.sprite = night;
            }
        }
    }
    public void IncreaseParallaxSpeed()
    {
        StartCoroutine(ParallaxSpeedUpRoutine());
    }

    IEnumerator ParallaxSpeedUpRoutine()
    {
        foreach(SpriteRenderer sr in backgroundElements)
        {
            float speed = sr.material.GetFloat("_ParallaxVelocity");
            Material m = sr.material;
            m.SetFloat("_ParallaxVelocity", speed + parallaxSpeedIncrease);
            for(float t = 0; t < 1; t += Time.deltaTime)
            {                                
                sr.material.Lerp(sr.material, m,t);
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
