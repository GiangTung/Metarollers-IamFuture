using Dreamteck.Splines;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public SplineComputer splineComputer;
    public Vector2 origin;
    public List<SplinePoint> points = new List<SplinePoint>();
    public SplineFollower follower;
    public float size;
    public Transform playerPos;
    public double percent;
    public double offset;
    public bool blocked;
    public Vector2 lastPos;
    
    void Start()
    {
        lastPos = playerPos.position;
        splineComputer.onRebuild += () =>
        {
            percent = (percent - (2d / 3d)) + (1d / 3d);
            Spawn.instance.SpawnObs(splineComputer);
        };
        size = splineComputer.GetPointSize(0);
        points = new List<SplinePoint>(splineComputer.GetPoints());

    }

    // Update is called once per frame
    void Update()
    {
        percent += follower.followSpeed * Time.deltaTime;
        
        if (percent >= 2d / 3d)
        {
            UpdateSpline();
        }
        follower.SetPercent(percent);

    }

    public void UpdateSpline()
    {
        blocked = true;
        // follower.enabled = false;
        points.RemoveAt(0);
        // adding point
        var lastPoint = splineComputer.GetPoint(splineComputer.pointCount - 1);
        Vector2 newPointPosition = new Vector2(lastPoint.position.x + 50, origin.y + Random.Range(-7f, 7f));
        SplinePoint point = new SplinePoint(newPointPosition);
        point.size = size;
        points.Add(point);
        //splineComputer.SetPoint(splineComputer.pointCount, point);
        splineComputer.SetPoints(points.ToArray());
        
       



    }

}
