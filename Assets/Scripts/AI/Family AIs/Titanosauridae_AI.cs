using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Titanosauridae_AI : Herbivourous_AI
{
    //the bool to see if its in a herd
    bool InHerd;
    //The levels at which it will start looking for resources
    public float ThirtinessLvl;
    public float StarvationLvl;

    void Start()
    {
        //The AI starts by getting its NavMesh agent from the gameObject
        Move = GetComponent<NavMeshAgent>();
        //Sets up its food, thirst and health
        Food = 1000;
        thirst = 1000;
        Hp = 100;
        //It them sets the first movement destination, this will be changed soon
        Move.destination = CalculateNextPos();
        //Grabs the other objects that are needed for general functioning
        AllObjectsNeeded();
    }

    // Update is called once per frame
    void Update()
    {
        //If its not in a herd itll wonder by itself
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
        //If its currently in a herd is not added yet
    }
}
