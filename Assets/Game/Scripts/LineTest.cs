using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cube1;
    public GameObject cube2;
    private LineRenderer lineRenderer;
    void Start()
    {
        GameObject line = new GameObject();
        line.AddComponent<LineRenderer>();
        lineRenderer = line.GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, cube1.transform.position);
        lineRenderer.SetPosition(1, cube2.transform.position);
        lineRenderer.useWorldSpace = true;
    }
}
