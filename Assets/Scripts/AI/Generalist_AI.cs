using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Generalist_AI : MonoBehaviour
{
    public float Food;
    public float thirst;
    public float speed;
    public int Size;

    Renderer[] Water;
    Renderer[] Animals;
    public Renderer[] Plants;

    public GameObject[] StarvingTargets;
    public GameObject[] NormalTargets;
    public enum CurrentAction {HUNGRY, HUNTING, GREASING, WATERING, IDLE, MOVING, DEAD }
    public CurrentAction CurAct;
    public enum Diet {HERBIVORE, CARNIVORE, OMNIVORE };
    public Diet diet;
    public void AllObjectsNeeded()
    {
        GameObject waterObjects = GameObject.Find("WaterHoles");
        GameObject animalObjects = GameObject.Find("AnimalsStorage");
        GameObject plantObjects = GameObject.Find("Plants");
        Water = waterObjects.GetComponentsInChildren<Renderer>();
        Animals = animalObjects.GetComponentsInChildren<Renderer>();
        Plants = plantObjects.GetComponentsInChildren<Renderer>();
    }
    protected Vector3 CalculateNextPos()
    {
        float x = Random.Range(transform.position.x - 50, transform.position.x + 50);
        float z = Random.Range(transform.position.z - 50, transform.position.z + 50);
        Vector3 Pos = new(x, 0, z);
        return Pos;
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
    }
    protected CurrentAction CheckLevels(Diet diet, float water, float food)
    {
        if (food < 500)
        {
            if (Random.Range(1, 1000) > 750)
            {
                switch (diet)
                {
                    case Diet.HERBIVORE:
                        return CurrentAction.GREASING;
                    case (Diet.CARNIVORE):
                        return CurrentAction.HUNTING;
                    case Diet.OMNIVORE:
                        if(Random.Range(1, 2) > 1)
                        {
                            return CurrentAction.HUNTING;
                        }
                        else
                            return CurrentAction.GREASING;
                }
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
    public GameObject LocatePlants(Camera[] Eyes, GameObject[] NormalPlants, GameObject[] StarvingPlants, float Food)
    {
        GameObject Pos = null;
        for (int i = 0; i < Eyes.Length; i++)
        {
            Debug.Log("Food Renderers = " + VisibleFoodRenderers(Plants, Eyes[i], NormalPlants, StarvingPlants, Food));
            Pos = VisibleFoodRenderers(Plants, Eyes[i], NormalPlants, StarvingPlants, Food);
        }
        if (Pos == null)
        {
            CalculateNextPos();
        }
        return Pos;
    }
    public GameObject LocatePrey(Camera[] Eyes, GameObject[] Prey, GameObject[] StarvingPrey, float Food)
    {
        GameObject Pos = null;
        for (int i = 0; i < Eyes.Length; i++)
        {
            Pos = VisibleFoodRenderers(Animals, Eyes[i], Prey, StarvingPrey, Food);
        }
        if(Pos == null)
        {
            CalculateNextPos();
        }
        return Pos;
    }
    public Vector3 LocateWater(Camera[] Eyes)
    {
        Vector3 Pos = new(100,100,100);
        for (int i = 0; i < Eyes.Length; i++)
        {
            Pos = VisibleWaterRenderers(Water, Eyes[i]);
        }
        return Pos;
    }

    private GameObject VisibleFoodRenderers(Renderer[] Renders, Camera Eye, GameObject[] Prey, GameObject[] StarvingPrey, float Food)
    {
        GameObject[] prey;
        GameObject Target = null;
        if (Food > 300)
            prey = Prey;       
        else
            prey = StarvingPrey;
        for (int i = 0; i < Renders.Length; i++)
        {
            if (IsVisible(Renders[i], Eye))
            {
                if (Target != null)
                {
                    if (Vector3.Distance(Renders[i].transform.position, transform.position) < Vector3.Distance(Target.transform.position, transform.position))
                    {
                        foreach (GameObject food in prey)
                        {
                            if (Renders[i].gameObject == food)
                            {
                                Target = Renders[i].gameObject;
                                Debug.Log("Target = " + Renders[i].GetComponent<GameObject>());
                            }
                        }
                    }
                }
                else
                {
                    foreach (GameObject food in prey)
                    {
                        if (Renders[i].gameObject == food)
                        {
                            Target = Renders[i].gameObject;
                            Debug.Log(Target);
                        }
                    }
                }
            }
        }
        return Target;
    }
    private Vector3 VisibleWaterRenderers(Renderer[] Renders, Camera Eye)
    {
        Vector3 OldPos = new(1000, 1000, 1000);
        Vector3 Pos = new(1000, 1000, 1000);
        for (int i = 0; i < Renders.Length; i++)
        {
            // output only the visible renderers' name
            if (IsVisible(Renders[i], Eye))
            {
                if (Vector3.Distance(Renders[i].transform.position, transform.position) < Vector3.Distance(Pos, transform.position))
                {
                    Pos = Renders[i].transform.position;
                }
            }
        }
        if (Pos == OldPos)
        {
            Pos = CalculateNextPos();
        }
        return Pos;
    }
    private bool IsVisible(Renderer renderer, Camera Eye)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Eye);
        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            return true;
        else
            return false;
    }
    protected bool CheckSurroundings(CurrentAction CurAct)
    {
        if (CurAct == CurrentAction.WATERING)
        {
            for (int i = 0; i < Water.Length; i++)
            {
                if (Vector3.Distance(Water[i].transform.position, transform.position) < 25)
                {
                    return true;
                }
            }
            return false;
        }
        if(CurAct == CurrentAction.GREASING)
        {
            for (int i = 0; i < Plants.Length; i++)
            {
                if (Vector3.Distance(Plants[i].transform.position, transform.position) < 35)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
}
