using UnityEngine;
using System.Collections;

public class GameManaging : MonoBehaviour
{
    public static GameManaging instance;
    
    [SerializeField] float levelCountdown;
    private float timer;
    private int inscreases;
    [SerializeField] int easySpawns, mediumSpawns, hardSpawns;
    public float[] lanePos;

    private void Awake()
    {
        inscreases = 1;
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= levelCountdown)
        {
            inscreases++;
            timer = 0;
        }
    }
    
    public int GetDifficulty()
    {
        if (inscreases == 1) return easySpawns;

        else if (inscreases == 2) return mediumSpawns;

        else return hardSpawns;

    }
}
