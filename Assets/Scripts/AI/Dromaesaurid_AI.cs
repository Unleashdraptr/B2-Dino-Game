using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dromaesaurid_AI : Generalist_AI
{
    public Camera Eyes;
    public Vector3 Movement;
    NavMeshAgent Move;
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
        Food -= 1 * Time.deltaTime;
        thirst -= 1 * Time.deltaTime;
        if (CurAct == CurrentAction.IDLE || CurAct == CurrentAction.MOVING)
        {
            CurAct = CheckLevels(diet, thirst, Food);
        }
        if(Food <= 0 || thirst <= 0)
        {
            CurAct = CurrentAction.DEAD;
        }
        if (Move.remainingDistance <= 1)
        {
            Move.destination = CalculateNextPos();
        }
    }
}
