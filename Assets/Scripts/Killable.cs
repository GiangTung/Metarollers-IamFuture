using UnityEngine;

public class Killable : MonoBehaviour
{
    public int lane;
    [SerializeField] bool isObstacle;
    [SerializeField] bool usingPolygon;
    PolygonCollider2D pc;
    BoxCollider2D col;

    private void Start()
    {
        if (usingPolygon) pc = GetComponent<PolygonCollider2D>();
        else col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!isObstacle) return;

        if (lane == Player.instance.lane)
        {
            if (usingPolygon) pc.enabled = true;
            else col.enabled = true;
        }
        else
        {
            if(usingPolygon) pc.enabled = false;
            else col.enabled = false;
        }
    }
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            GameController.instance.lifes = 0;
            GameController.instance.OnHit(2);
            Player.instance.OnDeath();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            GameController.instance.lifes = 0;
            GameController.instance.OnHit(2);
            Player.instance.OnDeath();
        }
    }
}
