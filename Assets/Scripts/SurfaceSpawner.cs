using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceSpawner : MonoBehaviour
{
    Player player;
    [SerializeField] List<GameObject> tiles;
    Transform nextPosition;
    [SerializeField] GameObject start;
    private int leftIndex = 0;
    private int rightIndex = 1;
    float playerX;

    private void Start()
    {
        player = Player.instance;
        playerX = player.transform.position.x + 50;
        nextPosition = start.transform.GetChild(rightIndex);
        
        for (int i = 0; i < 5; i++)
        {
            InstantiateTile();
        }
    }

    private void Update()
    {
        if(player.transform.position.x > playerX)
        {
            InstantiateTile();
            playerX += 10;
        }        
    }

    public void InstantiateTile()
    {
        int r = Random.Range(0, tiles.Count);
        GameObject g = Instantiate(tiles[r], nextPosition.position, tiles[r].transform.rotation);
        g.transform.position += (g.transform.position - g.transform.GetChild(leftIndex).position);
        nextPosition = g.transform.GetChild(rightIndex);
        tiles.Add(g);
    }


}
