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
    Renderer[] Plants;

    public GameObject[] StarvingTargets;
    public GameObject[] NormalTargets;
    public enum CurrentAction {HUNTING, GREASING, WATERING, IDLE, MOVING, DEAD }
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
        int x = Random.Range(-250, 250);
        int z = Random.Range(-250, 250);
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
    public Vector3 CheckForFood(Camera[] Eyes, bool EyeNum, Diet diet)
    {
        Renderer[] renderers = null;
        Vector3 Pos = new(0, 0, 0);
        Vector3 PlantPos = new(0, 0, 0);
        switch (diet)
        {
            case Diet.HERBIVORE:
                renderers = Plants;
                break;
            case (Diet.CARNIVORE):
                renderers = Animals;
                break;
            case Diet.OMNIVORE:
                renderers = Animals;
                break;
        }
        if (EyeNum)
        {
            for (int i = 0; i < Eyes.Length; i++)
            {
                Pos = VisibleRenderers(renderers, Eyes[i]);
            }
        }
        else
            Pos = VisibleRenderers(renderers, Eyes[0]);
        if (diet == Diet.OMNIVORE)
        {
            if (EyeNum)
            {
                for (int i = 0; i < Eyes.Length; i++)
                {
                    PlantPos = VisibleRenderers(Plants, Eyes[i]);
                }
            }
            else
                PlantPos = VisibleRenderers(Plants, Eyes[0]);
            if (Vector3.Distance(Pos, transform.position) > Vector3.Distance(PlantPos, transform.position))
            {
                Pos = PlantPos;
            }
        }
        return Pos;
    }
    //False = 1, true = 2
    public Vector3 FindNearestWater(Camera[] Eyes, bool EyeNum)
    {
        Vector3 Pos = new(0,0,0);
        if(EyeNum)
        {
            for(int i = 0; i < Eyes.Length; i++)
            {
                Pos = VisibleRenderers(Water, Eyes[i]);
            }
        }
        return Pos;
    }

    private Vector3 VisibleRenderers(Renderer[] Renders, Camera Eye)
    {
        Vector3 OldPos = new(1000, 1000, 1000);
        Vector3 Pos = new(1000, 1000, 1000);
        for(int i = 0; i < Renders.Length; i++)
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
        if(Pos == OldPos)
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
}
