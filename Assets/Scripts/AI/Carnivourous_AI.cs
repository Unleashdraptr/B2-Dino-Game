using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivourous_AI : Generalist_AI
{
    Transform PreyLocation;
    float ChaseTime;
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
    public void UpdateStates()
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
                    Move.destination = LocateWater(Eyes);
            }
            else if (CurAct == CurrentAction.HUNGRY || ChaseTime < 0 && CurAct == CurrentAction.HUNTING)
            {
                if (PreyLocation != null)
                {
                    if (Vector3.Distance(PreyLocation.position, transform.position) < 35)
                    {
                        Food += 250;
                        ChaseTime = 10f;
                        CurAct = CheckLevels(diet, thirst, Food);
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
        if (CurAct == CurrentAction.HUNTING)
        {
            ChaseTime -= 1 * Time.deltaTime;
        }
    }
}
