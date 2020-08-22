using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkMouseInput();
    }

    private void checkMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "Interactable")
                {
                    Debug.Log(hitInfo.transform.name + " selected");
                    hitInfo.transform.GetComponentInParent<Selectable>().isSelected = true;
                }
            }
        }
    }
}
