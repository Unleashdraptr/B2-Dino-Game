using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dromaesauridae_AI : Carnivourous_AI
{
    Pack_AI Pack;
    public float ThirtinessLvl;
    public float StarvationLvl;
    float EatTime;
    bool IsEating;
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
            if (InPack)
            {
                if (IsAlpha)
                {
                    //They will monitor their levels in CheckState() and will then update them accordingly
                    CheckState(Pack.PackAction, ThirtinessLvl, StarvationLvl);
                    Pack.PackNeeds[PackNum] = CurAct;
                    //In UpdateStates() they will also decide where they are going to move
                    if (Move.remainingDistance < 1 && (CurAct == CurrentAction.MOVING || CurAct == CurrentAction.IDLE))
                    {
                        Move.destination = CalculateNextPos();
                        Pack.AlphaWalkLocation = Move.destination;
                    }
                    if (CurAct != CurrentAction.IDLE || CurAct != CurrentAction.MOVING)
                    {
                        Pack.FindLargestNeed();
                        UpdateStates(Pack.PackAction, ThirtinessLvl, StarvationLvl, Pack.CombDangerLvl);
                        if (PreyLocation != null)
                        {
                            Pack.AlphaHuntTarget = PreyLocation;
                        }
                        else if (CurAct == CurrentAction.MOVING)
                        {
                            Pack.PackAction = CurrentAction.MOVING;
                        }
                    }
                }
                else
                {
                    Vector3 Offset = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                    //They will monitor their levels in CheckState() and will then update them accordingly
                    CheckState(CurAct, ThirtinessLvl, StarvationLvl);
                    Pack.PackNeeds[PackNum] = CurAct;
                    if (Move.remainingDistance < 1 && (CurAct == CurrentAction.MOVING || CurAct == CurrentAction.IDLE))
                    {
                        Move.destination = Pack.AlphaWalkLocation + Offset;
                    }
                    if (Pack.PackAction != CurrentAction.MOVING)
                    {
                        CurAct = Pack.PackAction;
                    }
                    else
                        UpdateStates(CurAct, ThirtinessLvl, StarvationLvl, Pack.CombDangerLvl);
                }
                if (Pack.AlphaHuntTarget != null)
                {
                    if (Pack.AlphaHuntTarget.GetComponent<Generalist_AI>().CurAct != CurrentAction.DEAD)
                    {
                        CurAct = CurrentAction.HUNTING;
                        Move.destination = Pack.AlphaHuntTarget.position;
                    }
                    else if (Pack.AlphaHuntTarget.GetComponent<Generalist_AI>().CurAct == CurrentAction.DEAD)
                    {
                        CurAct = CurrentAction.FEEDING;
                    }
                }
                else if (Pack.AlphaHuntTarget == null && CurAct == CurrentAction.FEEDING)
                {
                    Pack.PackAction = CurrentAction.MOVING;
                    CurAct = CurrentAction.MOVING;
                }
                if (CurAct == CurrentAction.FEEDING && EatTime <= 0)
                {
                    Generalist_AI food = Pack.AlphaHuntTarget.GetComponent<Generalist_AI>();
                    Move.destination = Pack.AlphaHuntTarget.position;
                    if (Food != 600)
                    {
                        Food += EatAmount;
                        if (Food > 600) { Food = 600; }
                        if (Hp != 100)
                        {
                            Hp += food.FoodValue / 10;
                            if (Hp > 100) { Hp = 100; }
                        }
                        food.FoodValue -= EatAmount;
                    }
                    EatTime = 10f;
                    IsEating = true;
                }
            }
            else
            {
                //They will monitor their levels in CheckState() and will then update them accordingly
                CheckState(CurAct, ThirtinessLvl, StarvationLvl);
                //In UpdateStates() they will also decide where they are going to move
                UpdateStates(CurAct, ThirtinessLvl, StarvationLvl, Pack.CombDangerLvl);
            }
        }
        else
            Move.speed = 0;
        if(IsEating == true)
        {
            EatTime -= 1 * Time.deltaTime;
        }
    }
}
