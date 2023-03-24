using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dromaesaurid_AI : Generalist_AI
{
    public Camera[] Eyes;
    public Vector3 Movement;
    public GameObject PreyLocation;
    NavMeshAgent Move;
    float ScoutTime;

    // Start is called before the first frame update
    void Start()
    {
        Move = GetComponent<NavMeshAgent>();
        Food = 1000;
        thirst = 1000;
        Move.destination = CalculateNextPos();
    }
    private void Update()
    {
        if (CurAct != CurrentAction.DEAD)
        {
            Food -= 2.5f * Time.deltaTime;
            thirst -= 2.5f * Time.deltaTime;
            if (CurAct == CurrentAction.IDLE || CurAct == CurrentAction.MOVING)
            {
                CurAct = CheckLevels(diet, thirst, Food);
            }
            if (Food <= 0 || thirst <= 0)
            {
                CurAct = CurrentAction.DEAD;
            }
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
                if (CurAct == CurrentAction.HUNGRY || ScoutTime < 0 && CurAct == CurrentAction.HUNTING)
                {
                    PreyLocation = LocatePrey(Eyes, NormalTargets, StarvingTargets, Food);
                    CurAct = CurrentAction.HUNTING;
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
        else
            Move.speed = 0;
        if(CurAct == CurrentAction.HUNTING)
        {
            ScoutTime -= 1 * Time.deltaTime;
        }
    }
}
