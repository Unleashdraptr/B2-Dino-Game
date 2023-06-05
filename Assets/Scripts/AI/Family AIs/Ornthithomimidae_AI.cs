using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ornthithomimidae_AI : Herbivourous_AI
{
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
        //Will stop if the animal is dead
        if (CurAct != CurrentAction.DEAD)
        {
            //While alive they will slowly starve and need water
            Food -= 5 * Time.deltaTime;
            thirst -= 5 * Time.deltaTime;
            //They will monitor their levels in CheckState() and will then update them accordingly
            CheckState(ThirtinessLvl, StarvationLvl);
            //In UpdateStates() they will also decide where they are going to move
            UpdateStates(ThirtinessLvl, StarvationLvl);
        }
        else
            Move.speed = 0;
        //If its been eaten then itll disappear
        if(FoodValue <= 0)
        {
            Destroy(gameObject);
        }
    }
}
