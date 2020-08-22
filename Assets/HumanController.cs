using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public GameObject waypointPositions;
    private List<Transform> waypointList = new List<Transform>();
    private SphereCollider detector;
    
    // Start is called before the first frame update
    void Start()
    {
        detector = GetComponentInChildren<SphereCollider>();
        foreach(Transform child in waypointPositions.transform)
        {
            waypointList.Add(child);
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        if (agent.velocity.magnitude == 0)
        {
            int a = Random.Range(0,waypointList.Count);
            agent.SetDestination(waypointList[a].position);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("hi"+ collider.gameObject.name);
        
    }
}
