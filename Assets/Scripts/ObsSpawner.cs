using System.Collections.Generic;
using UnityEngine;

public class ObsSpawner : MonoBehaviour
{
    public static ObsSpawner instance;
    
    [SerializeField] Selector selector;
    [SerializeField] Vector2 spawnInterval;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] Transform parent;
    public float obstacleSpeed = 3;
    public HashSet<Obstacle> obstacles = new HashSet<Obstacle>();
    
    [Header("Speed Button Settings")]
    [SerializeField] float spawnDecrease;
    
    private float timer;
    private bool isAlive = true;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EventManager.SpeedIncrease += OnSpeedButton;   
    }

    private void OnDisable()
    {
        EventManager.SpeedIncrease -= OnSpeedButton;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        timer = Random.Range(spawnInterval.x, spawnInterval.y);
        isAlive = true;          
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0 && isAlive)
        {
            InstantiateObstacle();
            timer = Random.Range(spawnInterval.x,spawnInterval.y);
        }
    }

    public void InstantiateObstacle()
    {
        int lane = Random.Range(0, spawnPositions.Length);
        Vector3 spawnPos = new Vector3(10,spawnPositions[lane].position.y, 0);
        Obstacle obstacle = Instantiate(selector.GetRandomElement(), spawnPos, Quaternion.identity).GetComponent<Obstacle>();
       // obstacle.SetLane(lane);
        obstacles.Add(obstacle);
    }

    public float GetTrackSpeed() => obstacleSpeed;

    public void RemoveObstacle(Obstacle obstacle) => obstacles.Remove(obstacle);
    
    public void OnDeath()
    {
        //foreach (Obstacle obstacle in obstacles) obstacle.SetSpeed(0);
            
        isAlive = false;
    }

    public void OnSpeedButton()
    {
        spawnInterval -= new Vector2(spawnDecrease, spawnDecrease);
    }
}
