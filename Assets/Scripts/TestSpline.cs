using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class TestSpline : MonoBehaviour
{
    public SplineComputer splineComputer;
    public Vector2 origin;
    public List<SplinePoint> points = new List<SplinePoint>();
    public SplineFollower follower;
    public float size;
    
    // Start is called before the first frame update
    void Start()
    {
        size = splineComputer.GetPointSize(0);
        points = new List<SplinePoint>(splineComputer.GetPoints());    
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddPoint();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            RemovePoint();    
        }
    }
    
    public void AddPoint()
    {
        var lastPoint = splineComputer.GetPoint(splineComputer.pointCount - 1);
        Vector2 newPointPosition = new Vector2(lastPoint.position.x + 5, origin.y + Random.Range(-2f, 2f));
        SplinePoint point = new SplinePoint(newPointPosition);
        point.size = size;
        points.Add(point);
        splineComputer.SetPoints(points.ToArray());
    }

    public void RemovePoint()
    {
        if (points.Count < 2) return;
        points.RemoveAt(0);
        splineComputer.SetPoints(points.ToArray());
    }

}
