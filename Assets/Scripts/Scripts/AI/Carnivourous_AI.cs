using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivourous_AI : Generalist_AI
{
    public int PackNum;
    public bool IsAlpha;
    public bool InPack;
    public float EatAmount;
    public Transform PreyLocation;
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
    public void UpdateStates(CurrentAction Current, float thirsting, float starve, int DngrLvl)
    {
        //if we were thirsty we use this
        if (Current == CurrentAction.WATERING)
        {
            //We check our surroundings to see if we are at a water source and if so to drink and check our levels again
            if (CheckSurroundings(CurAct))
            {
                thirst += 250;
                Current = CheckLevels(diet, thirst, Food, thirsting, starve);
            }
            //Else we find the closest water source visible and head over there
            else
            {
                Move.destination = LocateWater(Eyes);
            }
        }
        //We are hungry or are actively hunting a target we use this
        if (Current == CurrentAction.HUNGRY || (ChaseTime < 0 && Current == CurrentAction.HUNTING))
        {
            //If we have a preylocation then we will find said prey again and carry on with the cahse
            if (PreyLocation != null)
            {
                //If we are next to our prey we then eat them and check our levels again
                if (Vector3.Distance(PreyLocation.position, transform.position) < 35)
                {
                    Food += 250;
                    ChaseTime = 10f;
                    PreyLocation.GetComponent<Generalist_AI>().Hp -= 10;
                    Current = CheckLevels(diet, thirst, Food, thirsting, starve);
                }
            }
            else
            {
                //If we can see the prey  we then update to hunting and start to chase said prey
                if (LocatePrey(Eyes, NormalTargets, StarvingTargets, Food, DngrLvl) != null)
                {
                    PreyLocation = LocatePrey(Eyes, NormalTargets, StarvingTargets, Food, DngrLvl);
                    Current = CurrentAction.HUNTING;
                    Move.destination = PreyLocation.position;
                    ChaseTime = 10f;
                }
            }
        }
        //If we are hunting then the timer to reset the position will decrease
        if (Current == CurrentAction.HUNTING)
        {
            ChaseTime -= 1 * Time.deltaTime;
        }
    }
}
