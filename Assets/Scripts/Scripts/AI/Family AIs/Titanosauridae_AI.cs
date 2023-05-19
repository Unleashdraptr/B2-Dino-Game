using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Titanosauridae_AI : Herbivourous_AI
{
    bool InHerd;
    public float ThirtinessLvl;
    public float StarvationLvl;
    void Start()
    {
        Move = GetComponent<NavMeshAgent>();
        Food = 1000;
        thirst = 1000;
        Move.destination = CalculateNextPos();
        AllObjectsNeeded();
    }

    // Update is called once per frame
    void Update()
    {
        if (InHerd == false)
        {
            if (CurAct != CurrentAction.DEAD)
            {
                Food -= 5 * Time.deltaTime;
                thirst -= 5 * Time.deltaTime;
                CheckState(ThirtinessLvl, StarvationLvl);
                if (UpdateStates(ThirtinessLvl, StarvationLvl) == true)
                {
                    
                }
            }
            else
                Move.speed = 0;
        }
    }
}
