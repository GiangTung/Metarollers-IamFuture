using UnityEngine;

public class Street : MonoBehaviour
{
    public Transform [] laneBegin;

    public Transform[] laneEnd;

    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.DrawLine(laneBegin[i].position, laneEnd[i].position, Color.blue);
        }
        
    }

}
