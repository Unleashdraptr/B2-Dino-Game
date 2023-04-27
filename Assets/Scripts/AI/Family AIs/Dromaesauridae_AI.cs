using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dromaesauridae_AI : Carnivourous_AI
{
    Pack_AI Pack;
    public float ThirtinessLvl;
    public float StarvationLvl;
    //The AI starts by getting its NavMesh agent from the gameObject
    void Start()
    {
        if (transform.parent)
        {
            Pack = GetComponentInParent<Pack_AI>();
        }
        Move = GetComponent<NavMeshAgent>();
        //Sets up its food, thirst and health
        Food = 600;
        thirst = 600;
        Hp = 100;
        //It them sets the first movement destination, this will be changed soon
        Move.destination = CalculateNextPos();
        //Grabs the other objects that are needed for general functioning
        AllObjectsNeeded();
        if(Pack != null)
        {
            InPack = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Will stop if the animal is dead
        if (CurAct != CurrentAction.DEAD)
        {
            //While alive they will slowly starve and need water
            Food -= 1 * Time.deltaTime;
            thirst -= 1 * Time.deltaTime;
            CheckState(CurAct, ThirtinessLvl, StarvationLvl);
            if (IsAlpha && InPack)
            {
                //They will monitor their levels in CheckState() and will then update them accordingly
                CheckState(Pack.PackAction, ThirtinessLvl, StarvationLvl);
                Pack.PackNeeds[PackNum] = CurAct;
                //In UpdateStates() they will also decide where they are going to move
                if (Move.remainingDistance < 1)
                {
                    Move.destination = CalculateNextPos();
                    Pack.AlphaWalkLocation = Move.destination;
                }
            }
            else if (InPack && !IsAlpha)
            {
                Vector3 Offset = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                //They will monitor their levels in CheckState() and will then update them accordingly
                CheckState(CurAct, ThirtinessLvl, StarvationLvl);
                Pack.PackNeeds[PackNum] = CurAct;
                if (Move.remainingDistance < 1)
                {
                    Move.destination = Pack.AlphaWalkLocation + Offset;
                }
            }
            else
            {
                //They will monitor their levels in CheckState() and will then update them accordingly
                CheckState(CurAct, ThirtinessLvl, StarvationLvl);
                //In UpdateStates() they will also decide where they are going to move
                UpdateStates(ThirtinessLvl, StarvationLvl);
            }
        }
        else
            Move.speed = 0;
    }
}
