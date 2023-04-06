using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Herbivourous_AI : Generalist_AI
{
    public void CheckState()
    {
        if (CurAct == CurrentAction.IDLE || CurAct == CurrentAction.MOVING)
        {
            CurAct = CheckLevels(diet, thirst, Food);
        }
        if (Food <= 0 || thirst <= 0)
        {
            CurAct = CurrentAction.DEAD;
        }
    }
    // Update is called once per frame
    public bool UpdateStates()
    {
        if (Move.remainingDistance <= 1 || CurAct == CurrentAction.IDLE)
        {
            if (CurAct == CurrentAction.WATERING)
            {
                if (CheckSurroundings(CurAct))
                {
                    thirst += 250;
                    CurAct = CheckLevels(diet, thirst, Food);
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
                    CurAct = CheckLevels(diet, thirst, Food);
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
