using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjects : MonoBehaviour
{
    private bool suspicious;
    // Start is called before the first frame update
    void Start()
    {
        suspicious = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSuspicion (bool toChange)
    {
        suspicious = toChange;
    }

    public bool isSuspicious()
    {
        return suspicious;
    }
}
