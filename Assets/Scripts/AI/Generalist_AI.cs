using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Generalist_AI : MonoBehaviour
{
    public Camera[] Eyes;
    protected NavMeshAgent Move;
    public float Food;
    public float FoodValue;
    public int DangerLvl;
    public float thirst;
    Renderer[] Water;
    GameObject Animals;
    GameObject Plants;
    public string[] StarvingTargets;
    public string[] NormalTargets;

    public enum CurrentAction {HUNGRY, HUNTING, GRAZING, WATERING, IDLE, MOVING, DEAD }
    public CurrentAction CurAct;
    public enum Diet {HERBIVORE, CARNIVORE, OMNIVORE };
    public Diet diet;
    public void AllObjectsNeeded()
    {
        GameObject waterObjects = GameObject.Find("WaterHoles");
        Water = waterObjects.GetComponentsInChildren<Renderer>();
        Animals = GameObject.Find("AnimalsStorage");
        Plants = GameObject.Find("Plants");
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
                        return CurrentAction.GRAZING;
                    case (Diet.CARNIVORE):
                        return CurrentAction.HUNTING;
                    case Diet.OMNIVORE:
                        if(Random.Range(1, 2) > 1)
                        {
                            return CurrentAction.HUNTING;
                        }
                        else
                            return CurrentAction.GRAZING;
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
    public Vector3 LocatePlants(Camera[] Eyes, string[] NormalPlants, string[] StarvingPlants, float Food)
    {
        Transform Position = FindClosestFood(Plants, Eyes, NormalPlants, StarvingPlants, Food);
        float x = Random.Range(Position.position.x - (transform.lossyScale.x), Position.position.x + (transform.lossyScale.x));
        float z = Random.Range(Position.position.z - (transform.lossyScale.z), Position.position.z + (transform.lossyScale.z));
        Vector3 Pos = new(x, Position.position.y, z);
        return Pos;
    }
    public Transform LocatePrey(Camera[] Eyes, string[] Prey, string[] StarvingPrey, float Food)
    {
           return FindClosestFood(Animals, Eyes, Prey, StarvingPrey, Food);
    }
    public Vector3 LocateWater(Camera[] Eyes)
    {
        Vector3 Pos = new(100,100,100);
        for (int i = 0; i < Eyes.Length; i++)
        {
            Pos = VisibleWaterRenderers(Water, Eyes[i]);
        }
        float x = Random.Range(Pos.x - (Pos.x / 2.5f), Pos.x + (Pos.x / 2.5f));
        float z = Random.Range(Pos.z - (Pos.z / 2.5f), Pos.z + (Pos.z / 2.5f));
        Pos = new(x, Pos.y, z);
        return Pos;
       
    }
    Transform FindClosestFood(GameObject Renders, Camera[] Eye, string[] Prey, string[] StarvingPrey, float Food)
    {
        Renderer[] Locations = Renders.GetComponentsInChildren<Renderer>();
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        for (int k = 0; k < Eye.Length; k++)
        {
            foreach (Renderer R in Locations)
            {
                // output only the visible renderers' name
                if (IsVisible(R, Eye[k]))
                {
                    foreach (string P in Prey)
                    {
                        if (R.CompareTag(P))
                        {
                            float dist = Vector3.Distance(R.transform.position, currentPos);
                            if (dist < minDist)
                            {
                                tMin = R.transform;
                                minDist = dist;
                            }
                        }
                        else if (Food <= 300)
                        {
                            foreach (string S in StarvingPrey)
                            {
                                if (R.CompareTag(S))
                                {
                                    float dist = Vector3.Distance(R.transform.position, currentPos);
                                    if (dist < minDist)
                                    {
                                        tMin = R.transform;
                                        minDist = dist;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return tMin;
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
        if(CurAct == CurrentAction.GRAZING)
        {
            for (int i = 0; i < Plants.transform.childCount; i++)
            {
                if (Vector3.Distance(Plants.transform.GetChild(i).transform.position, transform.position) < 35)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
}
