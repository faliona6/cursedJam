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
    private float cooldownTime = 2f;

    [Range(0, 1)]
    public float percentHead = 0.4f;


    private float forceStrength = 500f;
    private Rigidbody rgbd;
    private bool initSelected = false;
    private Transform lineStartPoint;
    private Outline outline;

    private StateManager stateManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();
        lineRenderer = line.GetComponent<LineRenderer>();
        lineStartPoint = transform.Find("lineStartPoint");
        outline = GetComponent<Outline>();

        rgbd = GetComponentInChildren<Rigidbody>();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (stateManager.onCooldown) { return; }
        if (initSelected && Input.GetMouseButtonDown(0)) { AddForceToObject(); }
        if (isSelected) { 
            enableArrowLaunch();
            enableOutline();
        }
        GetComponentInChildren<SphereCollider>();
        // add force to object

        if (Input.GetKeyDown(KeyCode.X) && isSelected)
        {
            isSelected = false;
            stateManager.isSelected = false;
            disableOutline();
        }
    }

    // After action is used
    private void AddForceToObject()
    {
        Debug.Log("hello");
        Vector3 direction = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
        rgbd.AddForce(Vector3.Normalize(direction + new Vector3(0, 1, 0)) * forceStrength);
        isSelected = false;
        initSelected = false;
        lineRenderer.positionCount = 0;
        StartCoroutine(StartCooldown());
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

            // arrowhead stuff

            lineRenderer.startWidth = 0.3f;
            lineRenderer.positionCount = 4;
            Vector3 direction = hitPoint - startPos;
            Vector3 endPos = startPos + (Vector3.Normalize(direction) * distance);

            lineRenderer.widthCurve = new AnimationCurve(
                new Keyframe(0, 0.4f)
                , new Keyframe(0.999f - percentHead, 0.4f)  // neck of arrow
                , new Keyframe(1 - percentHead, 1f)  // max width of arrow head
                , new Keyframe(1, 0f));  // tip of arrow
            lineRenderer.SetPositions(new Vector3[] {
              startPos
              , Vector3.Lerp(startPos, endPos, 0.999f - percentHead)
              , Vector3.Lerp(startPos, endPos, 1 - percentHead)
              , endPos });


            //lineRenderer.materials[0] = lineMaterial;
            lineRenderer.material = lineMaterial;
            lineRenderer.materials[0].mainTextureScale = new Vector3(distance, 1, 1);
            lineRenderer.material.color = Color.blue;

            lineRenderer.useWorldSpace = true;
        }
        initSelected = true;

    }

    IEnumerator StartCooldown()
    {
        stateManager.onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        stateManager.onCooldown = false;
        disableOutline();
        stateManager.isSelected = false;
    }

    private void enableOutline()
    {
        outline.OutlineWidth = 5f;
        outline.OutlineColor = new Color(0.4f, 0, 0.5f);
    }

    private void disableOutline()
    {
        outline.OutlineWidth = 0f;
        outline.OutlineColor = new Color(1, 1, 1);
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
