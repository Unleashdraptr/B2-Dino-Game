using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sauropod_AI : Generalist_AI
{
    public bool ASSENDMYCHILD;
    public Camera[] Eyes;
    NavMeshAgent Move;
    // Start is called before the first frame update
    void Start()
    {
        Move = GetComponent<NavMeshAgent>();
        Food = 1000;
        thirst = 1000;
        Move.destination = CalculateNextPos();
        AllObjectsNeeded();
    }
    // Update is called once per frame
    private void Update()
    {
        if (CurAct != CurrentAction.DEAD)
        {
            Food -= 5 * Time.deltaTime;
            thirst -= 5 * Time.deltaTime;
            if (CurAct == CurrentAction.IDLE || CurAct == CurrentAction.MOVING)
            {
                CurAct = CheckLevels(diet, thirst, Food);
            }
            if (Food <= 0 || thirst <= 0)
            {
                CurAct = CurrentAction.DEAD;
                ASSENDMYCHILD = true;
            }
            if (Move.remainingDistance <= 1 || CurAct == CurrentAction.IDLE)
            {
                if (CurAct == CurrentAction.WATERING)
                {
                    if(CheckSurroundings(CurAct))
                    {
                        thirst += 250;
                        CurAct = CheckLevels(diet, thirst, Food);
                    }
                    else
                        Move.destination = LocateWater(Eyes);
                }
                if (CurAct == CurrentAction.GREASING)
                {
                    if (CheckSurroundings(CurAct))
                    {
                        Food += 250;
                        CurAct = CheckLevels(diet, thirst, Food);
                    }
                    else
                    {
                        Debug.Log("Plant Location = "+ LocatePlants(Eyes, NormalTargets, StarvingTargets, Food));
                        if(LocatePlants(Eyes, NormalTargets, StarvingTargets, Food).transform.position != null)
                        {
                            Move.destination = LocatePlants(Eyes, NormalTargets, StarvingTargets, Food).transform.position;
                        }
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
        else
            Move.speed = 0;
        if(ASSENDMYCHILD)
        {
            transform.Translate(0, 50, 0);
        }
    }
}
