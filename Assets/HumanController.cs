using System;
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
    private Boolean pursuing;
    private GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        detector = GetComponentInChildren<SphereCollider>();
        foreach(Transform child in waypointPositions.transform)
        {
            waypointList.Add(child);
            target = null;
        }
        pursuing = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude <= 0.05 && !pursuing)
        {
            int a = UnityEngine.Random.Range(0,waypointList.Count);
            agent.SetDestination(waypointList[a].position);
        }
        if (agent.velocity.magnitude <= 0.05 && pursuing)
        {
            pursuing = false;
            target.GetComponent<PlayerObjects>().changeSuspicion(false);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
       
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerObjects current = collider.gameObject.GetComponent<PlayerObjects>();
            Vector3 sight = collider.gameObject.transform.position - gameObject.transform.position;
            // Boolean hit = Physics.Raycast(transform.position, Vector3.Normalize(sight));

            Boolean canSee = !Physics.Raycast(transform.position, collider.gameObject.transform.position);
           
            if (current.isSuspicious() && canSee)
            {
                Debug.Log("I can see you" + collider.gameObject.name);
                pursuing = true;
                target = collider.gameObject;
                agent.SetDestination(collider.gameObject.transform.position);
            }
        }

        


    }

    
}
