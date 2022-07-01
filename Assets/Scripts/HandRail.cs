using UnityEngine;

public class HandRail : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Grinding");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Not Grinding");
    }
}
