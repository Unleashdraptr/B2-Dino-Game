using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omnivorous_AI : Generalist_AI
{
    Transform PreyLocation;
    float ScoutTime;
    // Update is called once per frame
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
    public void UpdateActions(float thirsting, float starve)
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
                    Move.destination = LocateWater(Eyes);
            }
            if (CurAct == CurrentAction.HUNGRY)
            {
                float minDist = Mathf.Infinity;
                Vector3 currentPos = transform.position;
                if (LocatePlants(Eyes, NormalTargets, StarvingTargets, Food) != null)
                {
                    minDist = Vector3.Distance(LocatePlants(Eyes, NormalTargets, StarvingTargets, Food), currentPos);
                }
                if (LocatePrey(Eyes, NormalTargets, StarvingTargets, Food) != null)
                {
                    if (minDist < Vector3.Distance(LocatePrey(Eyes, NormalTargets, StarvingTargets, Food).position, currentPos))
                    {
                        CurAct = CurrentAction.HUNTING;
                        PreyLocation = LocatePrey(Eyes, NormalTargets, StarvingTargets, Food);
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
                    if (LocatePlants(Eyes, NormalTargets, StarvingTargets, Food) != null)
                    {
                        Move.destination = LocatePlants(Eyes, NormalTargets, StarvingTargets, Food);
                    }
                    else
                        Move.destination = CalculateNextPos();
                }
            }
            if (ScoutTime < 0 && CurAct == CurrentAction.HUNTING)
            {
                if (Vector3.Distance(PreyLocation.position, transform.position) < 35)
                {
                    Food += 250;
                    CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
                }
                else
                {
                    if (LocatePrey(Eyes, NormalTargets, StarvingTargets, Food) != null)
                    {
                        PreyLocation = LocatePrey(Eyes, NormalTargets, StarvingTargets, Food);
                        CurAct = CurrentAction.HUNTING;
                        Move.destination = PreyLocation.position;
                        ScoutTime = 10f;
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
