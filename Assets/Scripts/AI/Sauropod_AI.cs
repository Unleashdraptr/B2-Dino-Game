using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauropod_AI : Generalist_AI
{
    public Camera LEye;
    public Camera REye;
    Rigidbody rb;
    public Vector3 Movement;
    // Start is called before the first frame update
    void Start()
    {
        Food = 1000;
        Water = 1000;
        rb = GetComponent<Rigidbody>();
        CalculateMovement(ref Movement);
    }
    // Update is called once per frame
    private void Update()
    {
        Food -= 1 * Time.deltaTime;
        Water -= 1 * Time.deltaTime;
        if (CurAct == CurrentAction.IDLE || CurAct == CurrentAction.MOVING)
        {
            CurAct = CheckLevels(diet, Water, Food);
        }
        if (Food <= 0 || Water <= 0)
        {
            CurAct = CurrentAction.DEAD;
        }
    }
    void FixedUpdate()
    {
        MovePlayer(gameObject, rb, speed, ref Movement);
    }
}
