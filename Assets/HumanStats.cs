using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanStats : MonoBehaviour
{
    private int health;
    private Boolean isAlive;

    // Start is called before the first frame update
    void Start()
    {
        health = 10;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            isAlive = false;
        }
    }
    
    public void decreaseHealth(int decrease)
    {
        health -= decrease;
    }

    public void death()
    {

    }

}
