using UnityEngine;
using Dreamteck.Splines;

public class Obstacle : MonoBehaviour
{
    Vector2 normal;

    public void SetPosition(SplineComputer splineComputer, int lane)
    {
        normal = splineComputer.GetPointNormal(splineComputer.pointCount - 1);
        float offset = GameManaging.instance.lanePos[lane];
        Vector2 position = splineComputer.GetPoint(splineComputer.pointCount - 1).position;
        position += Vector2.up * offset;
        transform.parent.position = position;
        //splineFollower.spline = splineComputer;
        //splineFollower.SetPercent(percentage);
        // splineComputer.nor
       // var offsetModifier = splineFollower.offsetModifier;
        //offsetModifier.AddKey(splineComputer.GetPointNormal(splineComputer.pointCount - 1) * offset, 0, 0);
        //Destroy(splineFollower);
    }

    private void Update()
    {
    }
}
