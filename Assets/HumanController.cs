using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{
    public GameObject waypointPositions;
    private List<Transform> waypointList = new List<Transform>();
    private SphereCollider detector;
    private Boolean pursuing;
    private GameObject target;
    private NavMeshAgent agent;


    private float distToObjectBeforeSpray = 5f;
    private ParticleSystem waterSpray;
    private bool isSprayingWater = false;
    private float waterCooldownTime = 2f;

    private StateManager stateManager;

    // animation and ragdoll
    private Rigidbody[] childRigidbodies;
    private Collider[] childColliders;



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

        waterSpray = GetComponentInChildren<ParticleSystem>();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        childRigidbodies = GetComponentsInChildren<Rigidbody>();
        childColliders = GetComponentsInChildren<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSprayingWater) { return; }
        if (agent.velocity.magnitude <= 0.05 && !pursuing)
        {
            int a = UnityEngine.Random.Range(0,waypointList.Count);
            agent.SetDestination(waypointList[a].position);
        }
        float distToTarget = target != null ? (transform.position - target.transform.position).magnitude : 10;
        if (pursuing && distToTarget < 6)
        {
            pursuing = false;
            target.GetComponent<PlayerObjects>().changeSuspicion(false);
            sprayWater();
        }

    }

    private void OnTriggerStay(Collider collider)
    {
       
        if (collider.gameObject.CompareTag("Interactable"))
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

    private void sprayWater()
    {
        // trigger animation
        isSprayingWater = true;
        waterSpray.Play();
        StartCoroutine(WaterSprayCooldown());

        // check if player is in there
        Selectable curTarget = target.GetComponent<Selectable>();
        if (curTarget.isSelected)
        {
            stateManager.curHealth -= 1;
            Debug.Log("curHealth: " + stateManager.curHealth);
        }
        // deduct player health
    }

    IEnumerator WaterSprayCooldown()
    {
        yield return new WaitForSeconds(waterCooldownTime);
        isSprayingWater = false;
    }

    private void toggleRigidBodies(bool isKinematic)
    {
        foreach (Rigidbody rgbd in childRigidbodies)
        {
            rgbd.isKinematic = isKinematic;
        }
    }

    private void toggleColliders(bool enabled)
    {
        foreach (Collider col in childColliders)
        {
            col.enabled = enabled;
        }
    }

    private void enableRagDoll()
    {
        toggleRigidBodies(true);
        toggleColliders(true);
        agent.enabled = false;
    }

    private void disableRagDoll()
    {
        toggleRigidBodies(false);
        toggleColliders(false);
        agent.enabled = true;

    }


}
