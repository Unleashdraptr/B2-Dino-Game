using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generalist_AI : MonoBehaviour
{
    public float Food;
    public float Water;
    public float speed;
    public enum CurrentAction {HUNTING, GREASING, WATERING, IDLE, MOVING, DEAD }
    public CurrentAction CurAct;
    public enum Diet {HERBIVORE, CARNIVORE, OMNIVORE };
    public Diet diet;
    public enum Walk {QUADRAPED, BIPED, NONE }
    public Walk walk;
    protected void CalculateMovement(ref Vector3 PointOfInterest)
    {
        int x = Random.Range(-100, 100);
        int z = Random.Range(-100, 100);
        PointOfInterest = new(x, 0, z);
    }
    protected void MovePlayer(GameObject Animal, Rigidbody rb, float Speed, ref Vector3 PointOfInterest)
    {
        Animal.transform.LookAt(PointOfInterest);
        float Offset = PointOfInterest.x / PointOfInterest.z;
        float x = -Speed / Offset;
        float z = -Speed - x;
        //Total Move Direction for the frame
        Vector3 MoveDir = new(x, 0, z);
        //Setting the Y velocity aswell
        MoveDir.y = rb.velocity.y;
        //Calculates and then applies then input with rigidbody's Physics
        rb.MovePosition(transform.position + MoveDir * Time.deltaTime);
        if(Vector3.Distance(Animal.transform.position, PointOfInterest) < 10)
        {
            CalculateMovement(ref PointOfInterest);
        }
    }
    protected CurrentAction CheckLevels(Diet yes, float water, float food)
    {
        if (food < 500)
        {
            if (Random.Range(1, 1000) > 750)
            {
                if (yes == Diet.HERBIVORE)
                {
                    return CurrentAction.GREASING;
                }
                else if (yes == Diet.CARNIVORE)
                {
                    return CurrentAction.HUNTING;
                }
                else
                    CheckForFood();
            }
        }
        if (water < 500)
        {
            if (Random.Range(1, 1000) > 750)
            {
                return CurrentAction.WATERING;
            }
        }
        return CurrentAction.MOVING;
    }
    void CheckForFood()
    {
    
    }
}
