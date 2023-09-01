using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    [SerializeField] Segment[] segments;

    [SerializeField] Dictionary<Vector3, Transform> nodes;
    void Start()
    {
        int segment_num = transform.childCount;
        segments = new Segment[segment_num];

        nodes = new Dictionary<Vector3, Transform>();

        for(int i = 0; i < segment_num; i++)
        {
            Segment s = transform.GetChild(i).GetComponent<Segment>();
            segments[i] = s;

            Transform node1 = s.GetStart();
            Transform node2 = s.GetEnd();
            nodes.Add(node1.position, node1);
            nodes.Add(node2.position, node2);
        }


    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        i++;
    }
}
