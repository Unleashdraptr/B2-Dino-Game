using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivourous_AI : Generalist_AI
{
    public int PackNum;
    public bool IsAlpha;
    public bool InPack;
    Transform PreyLocation;
    float ChaseTime;
    public void CheckState(CurrentAction Action, float thirsting, float starve)
    {
        if (Action == CurrentAction.IDLE || Action == CurrentAction.MOVING)
        {
            CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
        }
        if (Food <= 0 || thirst <= 0)
        {
            CurAct = CurrentAction.DEAD;
        }
    }
    public bool UpdateStates(float thirsting, float starve)
    {
        if (Move.remainingDistance <= 1 || CurAct == CurrentAction.IDLE)
        {
            if (CurAct == CurrentAction.WATERING)
            {
                if (CheckSurroundings(CurAct))
                {
                    thirst += 250;
                    CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
                }
                else
                {
                    Move.destination = LocateWater(Eyes);
                    return false;
                }
            }
            if (CurAct == CurrentAction.HUNGRY || ChaseTime < 0 && CurAct == CurrentAction.HUNTING)
            {
                if (PreyLocation != null)
                {
                    if (Vector3.Distance(PreyLocation.position, transform.position) < 35)
                    {
                        Food += 250;
                        ChaseTime = 10f;
                        CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
                    }
                }
                else
                {
                    if (LocatePrey(Eyes, NormalTargets, StarvingTargets, Food) != null)
                    {
                        PreyLocation = LocatePrey(Eyes, NormalTargets, StarvingTargets, Food);
                        CurAct = CurrentAction.HUNTING;
                        Move.destination = PreyLocation.position;
                        ChaseTime = 10f;
                        return false;
                    }
                    else
                        return true;
                }
            }
            else
                return true;
        }
        if (CurAct == CurrentAction.HUNTING)
        {
            ChaseTime -= 1 * Time.deltaTime;
        }
        return false;
    }
}
