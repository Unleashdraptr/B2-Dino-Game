using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dromaesauridae_AI : Carnivourous_AI
{
    //The pack is belongs to
    Pack_AI Pack;
    //The levels at which they will actively start looking for these resources
    public float ThirtinessLvl;
    public float StarvationLvl;
    //Getting the Animations and it Physics
    Rigidbody rb;
    Animator animate;
    //Checks if its eating
    float EatTime;
    bool IsEating;
    //The AI starts by getting its NavMesh agent from the gameObject
    void Start()
    {
        //Gets both the animation and the animator for the model
        rb = GetComponent<Rigidbody>();
        animate = transform.GetChild(0).GetComponent<Animator>();
        //If the animal is stored in a parent, it'll be its pack
        if (transform.parent)
        {
            Pack = GetComponentInParent<Pack_AI>();
        }
        //Gets it NavMesh for movement
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
            //if the animal is moving forward itll run
            if (rb.velocity.x != 0)
            {
                animate.SetBool("IsRunning", true);
            }
            else
                animate.SetBool("IsRunning", true);

            //While alive they will slowly starve and need water and check if it needs to eat and/or drink
            Food -= 1 * Time.deltaTime;
            thirst -= 1 * Time.deltaTime;
            CheckState(CurAct, ThirtinessLvl, StarvationLvl);
            //If its in a pack itll think like a pack animal
            if (InPack)
            {
                //If its the alpha then itll lead them
                if (IsAlpha)
                {
                    //They will monitor their levels in CheckState() and will then update them accordingly
                    CheckState(Pack.PackAction, ThirtinessLvl, StarvationLvl);
                    Pack.PackNeeds[PackNum] = CurAct;
                    //In UpdateStates() they will also decide where they are going to move
                    if (Move.remainingDistance < 1 && (CurAct == CurrentAction.MOVING || CurAct == CurrentAction.IDLE))
                    {
                        //Gives the destination to the other pack members to go near
                        Move.destination = CalculateNextPos();
                        Pack.AlphaWalkLocation = Move.destination;
                    }
                    if (CurAct != CurrentAction.IDLE || CurAct != CurrentAction.MOVING)
                    {
                        //Itll check what the other pack members need
                        Pack.FindLargestNeed();
                        //Itll then update its own state accordingly
                        UpdateStates(Pack.PackAction, ThirtinessLvl, StarvationLvl, Pack.CombDangerLvl);
                        //If its hungry then itll look for prey, if it finds some then the pack will go for it
                        if (PreyLocation != null)
                        {
                            Pack.AlphaHuntTarget = PreyLocation;
                        }
                        //Else just move
                        else if (CurAct == CurrentAction.MOVING)
                        {
                            Pack.PackAction = CurrentAction.MOVING;
                        }
                    }
                }
                //If theyre not the Alpha but still in the pack
                else
                {
                    //The offset from the Alphas target position
                    Vector3 Offset = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                    //They will monitor their levels in CheckState() and will then update them accordingly
                    CheckState(CurAct, ThirtinessLvl, StarvationLvl);
                    //Tells the alpha its current state
                    Pack.PackNeeds[PackNum] = CurAct;
                    //Walking to the Alpha location + an offset
                    if (Move.remainingDistance < 1 && (CurAct == CurrentAction.MOVING || CurAct == CurrentAction.IDLE))
                    {
                        Move.destination = Pack.AlphaWalkLocation + Offset;
                    }
                    //If its not moving use the pack action
                    if (Pack.PackAction != CurrentAction.MOVING)
                    {
                        CurAct = Pack.PackAction;
                    }
                    //Else just update its states
                    else
                        UpdateStates(CurAct, ThirtinessLvl, StarvationLvl, Pack.CombDangerLvl);
                }
                //If the alpha hasnt found a target look for itself or its feeding off their prey they killed
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
                //They now ate the target and will return to moving
                else if (Pack.AlphaHuntTarget == null && CurAct == CurrentAction.FEEDING)
                {
                    Pack.PackAction = CurrentAction.MOVING;
                    CurAct = CurrentAction.MOVING;
                }
                //If theyre still eating and can eat again
                if (CurAct == CurrentAction.FEEDING && EatTime <= 0)
                {
                    //They get the food amount from the kill
                    Generalist_AI food = Pack.AlphaHuntTarget.GetComponent<Generalist_AI>();
                    Move.destination = Pack.AlphaHuntTarget.position;
                    //If they're not full, they will eat 
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
            //This is if theyre by themselves
            else
            {
                //They will monitor their levels in CheckState() and will then update them accordingly
                CheckState(CurAct, ThirtinessLvl, StarvationLvl);
                //In UpdateStates() they will also decide where they are going to move
                UpdateStates(CurAct, ThirtinessLvl, StarvationLvl, DangerLvl);
            }
        }
        //If they died dont move
        else
            Move.speed = 0;
        //Timer until it can eat again
        if(IsEating == true)
        {
            EatTime -= 1 * Time.deltaTime;
        }
    }
}
