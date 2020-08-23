using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjects : MonoBehaviour
{
    private Boolean suspicious;
    // Start is called before the first frame update
    void Start()
    {
        suspicious = true;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSuspicion (Boolean toChange)
    {
        suspicious = toChange;
    }

    public Boolean isSuspicious()
    {
        return suspicious;
    }
}
