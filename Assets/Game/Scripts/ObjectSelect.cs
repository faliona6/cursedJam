using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelect : MonoBehaviour
{
    private GameObject curHover = null;
    private StateManager stateManager;
    // Start is called before the first frame update
    void Start()
    {
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // if there is currently an object selected, return
        if (stateManager.isSelected) { return; }
        checkMouseInput();
    }

    private void checkMouseInput()
    {
        // if we're on cooldown, don't do anything
        if (stateManager.onCooldown) { return; }

        // check if hover on object
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

        if (hit && hitInfo.transform.gameObject.tag == "Interactable")
        {
            if (curHover == null)
            {
                curHover = hitInfo.transform.gameObject;
                curHover.GetComponent<Outline>().OutlineWidth = 5;
            }
        }
        else if ((!hit && curHover != null) || (hit && hitInfo.transform.gameObject.tag != "Interactable" && curHover != null))
        {
            curHover.GetComponent<Outline>().OutlineWidth = 0;
            curHover = null;
        }

        // check if clicked on object
        if (Input.GetMouseButtonDown(0))
        {
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "Interactable")
                {
                    stateManager.isSelected = true;
                    Debug.Log(hitInfo.transform.name + " selected");
                    hitInfo.transform.GetComponentInParent<Selectable>().isSelected = true;
                }
            }
        }
    }
}
