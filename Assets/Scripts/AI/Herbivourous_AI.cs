using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Herbivourous_AI : Generalist_AI
{
    public void CheckState(float thirsting, float starve)
    {
        if (CurAct == CurrentAction.IDLE || CurAct == CurrentAction.MOVING)
        {
            CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
        }
        if (Food <= 0 || thirst <= 0)
        {
            CurAct = CurrentAction.DEAD;
        }
    }
    // Update is called once per frame
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
            if (CurAct == CurrentAction.GRAZING)
            {
                if (CheckSurroundings(CurAct))
                {
                    Food += 250;
                    CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
                }
                else
                {
                    if (LocatePlants(Eyes, NormalTargets, StarvingTargets, Food) != null)
                    {
                        Move.destination = LocatePlants(Eyes, NormalTargets, StarvingTargets, Food);
                        return false;
                    }
                    else
                        return true;
                }
            }
        }
        return false;
    }
}
