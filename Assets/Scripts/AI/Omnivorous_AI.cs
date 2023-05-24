using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omnivorous_AI : Generalist_AI
{
    //How much the animal can eat in one bite
    public float EatAmount;
    //Location of their prey
    public Transform PreyLocation;
    //How long until they check where their prey was
    float ChaseTime;

    //This is how the Carnivores check their current state by inheriting this Script
    public void CheckState(float thirsting, float starve)
    {
        //If their moving or idle then check their levels
        if (CurAct == CurrentAction.IDLE || CurAct == CurrentAction.MOVING)
        {
            CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
        }
        //If any are below 0 kill them
        if (Food <= 0 || thirst <= 0 || Hp <= 0)
        {
            CurAct = CurrentAction.DEAD;
        }
    }
    // Same goes for UpdateStates, it uses both the herbivore and Carnivore states within its code since it both of them
    public void UpdateActions(float thirsting, float starve)
    {
        //If their at their destination or are just idle then check if they can drink or eat.
        if (Move.remainingDistance <= 1 || CurAct == CurrentAction.IDLE)
        {
            //If theyre thirsty
            if (CurAct == CurrentAction.WATERING)
            {
                //At water supply
                if (CheckSurroundings(CurAct))
                {
                    thirst += 250;
                    CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
                }
                //Not at water supply
                else
                    Move.destination = LocateWater(Eyes);
            }
            if (CurAct == CurrentAction.HUNGRY)
            {
                //At Food supply
                float minDist = Mathf.Infinity;
                Vector3 currentPos = transform.position;
                if (LocatePlants(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl) != null)
                {
                    minDist = Vector3.Distance(LocatePlants(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl), currentPos);
                }
                if (LocatePrey(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl) != null)
                {
                    if (minDist < Vector3.Distance(LocatePrey(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl).position, currentPos))
                    {
                        CurAct = CurrentAction.HUNTING;
                        PreyLocation = LocatePrey(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl);
                    }
                    else
                    {
                        CurAct = CurrentAction.GRAZING;
                    }
                }
            }
            if (CurAct == CurrentAction.GRAZING)
            {
                if (CheckSurroundings(CurAct))
                {
                    Food += 250;
                    CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
                }
                else
                {
                    if (LocatePlants(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl) != null)
                    {
                        Move.destination = LocatePlants(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl);
                    }
                    else
                        Move.destination = CalculateNextPos();
                }
            }
            if (ChaseTime < 0 && CurAct == CurrentAction.HUNTING)
            {
                if (Vector3.Distance(PreyLocation.position, transform.position) < 35)
                {
                    Food += 250;
                    CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
                }
                else
                {
                    if (LocatePrey(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl) != null)
                    {
                        PreyLocation = LocatePrey(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl);
                        CurAct = CurrentAction.HUNTING;
                        Move.destination = PreyLocation.position;
                        ChaseTime = 10f;
                    }
                    else
                        Move.destination = CalculateNextPos();
                }
            }
            else
            {
                if (Random.Range(1, 1000) > 800)
                {
                    Move.destination = CalculateNextPos();
                }
                else
                    CurAct = CurrentAction.IDLE;
            }
        }
    }
}
