using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dromaesauridae_AI : Carnivourous_AI
{
    void Start()
    {
        Move = GetComponent<NavMeshAgent>();
        Food = 1000;
        thirst = 1000;
        Move.destination = CalculateNextPos();
        AllObjectsNeeded();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurAct != CurrentAction.DEAD)
        {
            Food -= 5 * Time.deltaTime;
            thirst -= 5 * Time.deltaTime;
            CheckState();
            UpdateStates();
        }
        else
            Move.speed = 0;
    }
}
