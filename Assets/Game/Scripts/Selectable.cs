using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isSelected = false;
    public float distance = 4f;
    public float lineHeight = 2f;
    public Material lineMaterial;
    private LineRenderer lineRenderer;

    private float forceStrength = 500f;
    private Rigidbody rgbd;
    private bool initSelected = false;
    private Transform lineStartPoint;
    // Start is called before the first frame update
    void Start()
    {
        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();
        lineRenderer = line.GetComponent<LineRenderer>();
        lineStartPoint = transform.Find("lineStartPoint");

        rgbd = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (initSelected && Input.GetMouseButtonDown(0)) { AddForceToObject(); }
        if (isSelected) { enableArrowLaunch(); }
        GetComponentInChildren<SphereCollider>();
        // add force to object

        if (Input.GetKeyDown(KeyCode.X) && isSelected)
        {
            isSelected = false;
        }
    }

    private void AddForceToObject()
    {
        Debug.Log("hello");
        Vector3 direction = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
        rgbd.AddForce(Vector3.Normalize(direction + new Vector3(0, 1, 0)) * forceStrength);
        isSelected = false;
        initSelected = false;
        lineRenderer.positionCount = 0;
    }

    void enableArrowLaunch()
    {
        RaycastHit hit;
        lineRenderer.positionCount = 0;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.y = lineHeight;
            Vector3 startPos = new Vector3(lineStartPoint.position.x, lineHeight, lineStartPoint.position.z);

            //lineRenderer.useWorldSpace = true;

            lineRenderer.startWidth = 0.3f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            Vector3 direction = hitPoint - startPos;

            lineRenderer.SetPosition(1, startPos + (Vector3.Normalize(direction) * distance));
            lineRenderer.materials[0] = lineMaterial;
            lineRenderer.materials[0].mainTextureScale = new Vector3(distance, 1, 1);
            lineRenderer.useWorldSpace = true;
        }
        initSelected = true;
        /*
        Debug.Log(screenPoint);

        screenPoint.y = 0.5f;


        Plane objPlane = new Plane(Vector3.up, transform.position);

        Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray mouseRay = GenerateMouseRay();
        float rayDistance;
        objPlane.Raycast(mouseRay, out rayDistance);
        Vector3 m0 = transform.position - mRay.GetPoint(rayDistance);

        mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (objPlane.Raycast(mRay, out rayDistance))
        {
            screenPoint = mRay.GetPoint(rayDistance) + m0;
        }


        GameObject line = new GameObject();
        line.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;

        lineRenderer.GetComponent<LineRenderer>().startWidth = 0.05f;
        lineRenderer.GetComponent<LineRenderer>().positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, screenPoint);
        Debug.Log(screenPoint);
        */

    }

    Ray GenerateMouseRay()
    {
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray mr = new Ray(mousePosN, mousePosF - mousePosN);
        return mr;
    }
}
