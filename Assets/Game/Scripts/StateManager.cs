using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Manages state
public class StateManager : MonoBehaviour
{
    public bool onCooldown = false;
    public bool isSelected = false;

    public int startingHealth = 3;
    public int curHealth;

    private void Start()
    {
        curHealth = startingHealth;
    }
}
