using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField]
    public Transform[] points;

    public Segment[] prev_segments;
    public Segment[] next_segments;

    public float sqr_length;

    // Start is called before the first frame update
    void Start()
    {
        sqr_length = Vector3.SqrMagnitude(points[0].position - points[3].position); 
    }

    public Transform GetStart()
    {
        return points[0];
    }

    public Transform GetEnd()
    {
        return points[3];
    }

    public Vector3 BezierCurvePosition(float t)
    {
        Vector3 result = Vector3.zero;

        Vector3 a = Vector3.Lerp(points[0].position, points[1].position, t);
        Vector3 b = Vector3.Lerp(points[1].position, points[2].position, t);
        Vector3 c = Vector3.Lerp(points[2].position, points[3].position, t);

        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        result = Vector3.Lerp(ab, bc, t);

        return result;
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(points[0].position, points[1].position);
        Gizmos.DrawLine(points[1].position, points[2].position);
        Gizmos.DrawLine(points[2].position, points[3].position);

        for(float t = 0; t < 1; t += 0.02f)
        {
            Gizmos.DrawIcon(BezierCurvePosition(t), "DotFill", false);
        }
    }
}
