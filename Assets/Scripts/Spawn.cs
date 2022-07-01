using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;

public class Spawn : MonoBehaviour
{
    public static Spawn instance;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] Transform[] lanes;
    [SerializeField] SplineComputer splineComputer;
    
    private int spawnsCount;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnObs(SplineComputer splineComputer)
    {
        GameObject g = Instantiate(obstacles[Random.Range(0, obstacles.Length)]);
        g.GetComponentInChildren<Obstacle>().SetPosition(splineComputer, Random.Range(0,3));
        
        
        
    }
    

}
