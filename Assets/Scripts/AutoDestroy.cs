using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = Player.instance;
    }

    private void Update()
    {
        if (player.transform.position.x > transform.GetChild(1).transform.position.x +40)
            Destroy(gameObject);
    }
}
