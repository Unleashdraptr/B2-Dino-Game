using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Herbivourous_AI : Generalist_AI
{
    //This is how the Herbivores check their current state by inheriting this Script 
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
    // Same goes for UpdateStates
    public bool UpdateStates(float thirsting, float starve)
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
                {
                    Move.destination = LocateWater(Eyes);
                    return false;
                }
            }
            //If theyre hungry
            if (CurAct == CurrentAction.GRAZING)
            {
                //At food source
                if (CheckSurroundings(CurAct))
                {
                    Food += 250;
                    CurAct = CheckLevels(diet, thirst, Food, thirsting, starve);
                }
                //Not at food source
                else
                {
                    if (LocatePlants(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl) != null)
                    {
                        Move.destination = LocatePlants(Eyes, NormalTargets, StarvingTargets, Food, DangerLvl);
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
