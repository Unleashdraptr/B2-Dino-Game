using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Generalist_AI : MonoBehaviour
{
    //This script stores many of the stats and general things all animals need

    //Their Hp, food, danger level and water are all here, used for their decisions
    public float Hp;
    public float Food;
    public float FoodValue;
    public int DangerLvl;
    public float thirst;

    //This is used for movment
    public Camera[] Eyes;
    protected NavMeshAgent Move;

    //If its a quest animal
    public bool DontDespawn;

    //These are the objects they will check if they can see
    Renderer[] Water;
    GameObject Animals;
    GameObject Plants;

    //This is what they will eat during gameplay, reflecting a diet
    public string[] StarvingTargets;
    public string[] NormalTargets;

    //This stores their current action, this is how they decide to do
    public enum CurrentAction {HUNGRY, HUNTING, GRAZING, WATERING, IDLE, MOVING, DEAD, FEEDING }
    public CurrentAction CurAct;

    //This is used in the function in this script to say what diet it is
    public enum Diet {HERBIVORE, CARNIVORE, OMNIVORE };
    public Diet diet;

    //This is run during the start for all animals
    protected void AllObjectsNeeded()
    {
        GameObject waterObjects = GameObject.Find("Ocean");
        Water = waterObjects.GetComponentsInChildren<Renderer>();
        Animals = GameObject.Find("AnimalsStorage");
        Plants = GameObject.Find("Plants");
    }
    //This is when the player hits then or they are attacked by another animal
    public void TakeDmg(int Damage)
    {
        Hp -= Damage;
        if (Hp <= 0)
        {
            CurAct = CurrentAction.DEAD;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.rotation = new(transform.rotation.x, transform.rotation.y, 90, transform.rotation.w);
            enabled = false;
        }
    }
    //This is how they calculate their new position
    protected Vector3 CalculateNextPos()
    {
        float x = Random.Range(transform.position.x - 10, transform.position.x + 10);
        float z = Random.Range(transform.position.z - 10, transform.position.z + 10);
        //The Y is more difficult to get and is found within this function
        float y = CalculateHeight(x, z);
        Vector3 Pos = new(x, y, z);
        return Pos;
    }
    //This is how to get the Y position
    float CalculateHeight(float x, float z)
    {
        //It sets a point the same as the destination but 1000 units up
        Vector3 Pos = new(x, 1000, z);
        float y = 0;
        //It then shoots a line (called a Raycast) down to the ground with a distance of 1000
        if (Physics.Raycast(Pos, Vector3.down, out RaycastHit hit, 1000))
        {
            //This means that 1000 - hit.distance will equal the height of the terrain at that point. If it hits nothing then get a new position
            if (1000 - hit.distance <= 0)
            {
                CalculateNextPos();
            }
            else
                y = 1000 - hit.distance;
        }
        return y;
    }
    /* Unused
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
    */
    //This where the animals have been checking their states
    protected CurrentAction CheckLevels(Diet diet, float water, float food, float ThirstThreshold, float StarveThreshold)
    {
        //It first checks if the animal is hungry enough to start to look for food
        if (food < StarveThreshold)
        {
            //It then give a 25% chance itll start to look for food
            if (Random.Range(1, 1000) > 750)
            {
                //Itll then check the diet of the animal and then assign the correct action
                switch (diet)
                {
                    case Diet.HERBIVORE:
                        return CurrentAction.GRAZING;
                    case Diet.CARNIVORE:
                        return CurrentAction.HUNGRY;
                    case Diet.OMNIVORE:
                        if(Random.Range(1, 2) > 1)
                        {
                            return CurrentAction.HUNGRY;
                        }
                        else
                            return CurrentAction.GRAZING;
                }
            }
        }
        //it then checks if the animal is thirsty
        if (water < ThirstThreshold)
        {
            //if so, it then has a 25% chance to start looking for a source of water
            if (Random.Range(1, 1000) > 750)
            {
                return CurrentAction.WATERING;
            }
        }
        //else just move again
        return CurrentAction.MOVING;
    }
    //This is a herbivore only function that uses the animals Eyes to find the closest plant it can eat
    public Vector3 LocatePlants(Camera[] Eyes, string[] NormalPlants, string[] StarvingPlants, float Food, int DangerLvl)
    {
        //This function finds it
        Transform Position = FindClosestFood(Plants, Eyes, NormalPlants, StarvingPlants, Food, DangerLvl);
        //This then moves it back so the face it at the plant, not the centre of the animal
        float x = Random.Range(Position.position.x - (transform.lossyScale.x), Position.position.x + (transform.lossyScale.x));
        float z = Random.Range(Position.position.z - (transform.lossyScale.z), Position.position.z + (transform.lossyScale.z));
        Vector3 Pos = new(x, Position.position.y, z);
        return Pos;
    }
    //This is a Carnivore only function that uses the animals Eyes to find the closest prey that it can eat
    public Transform LocatePrey(Camera[] Eyes, string[] Prey, string[] StarvingPrey, float Food, int DangerLvl)
    {
        //It is found in this Function
        return FindClosestFood(Animals, Eyes, Prey, StarvingPrey, Food, DangerLvl);
    }
    //This function uses the animals eyes to find a source of water, this will be changed in the future
    public Vector3 LocateWater(Camera[] Eyes)
    {
        //It loops through all posible locations to find the closest one
        Vector3 Pos = new(100,100,100);
        for (int i = 0; i < Eyes.Length; i++)
        {
            Pos = VisibleWaterRenderers(Water, Eyes[i]);
        }
        //It then randomises it a little bit
        float x = Random.Range(Pos.x - (Pos.x / 2.5f), Pos.x + (Pos.x / 2.5f));
        float z = Random.Range(Pos.z - (Pos.z / 2.5f), Pos.z + (Pos.z / 2.5f));
        Pos = new(x, Pos.y, z);
        return Pos;
    }
    //This is how both find the closest food to them
    Transform FindClosestFood(GameObject Renders, Camera[] Eye, string[] Prey, string[] StarvingPrey, float Food, int DangerLvl)
    {
        //It runs similar to water, it also checks if its one the animal can eat from its diet
        //It gets all renders it can and then set the current one it targets to infinity
        Renderer[] Locations = Renders.GetComponentsInChildren<Renderer>();
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        //It loops this for each eye it has, so max 2 times
        for (int k = 0; k < Eye.Length; k++)
        {
            //It then loops through each render
            foreach (Renderer R in Locations)
            {
                // checks if the output is only the eye
                if (IsVisible(R, Eye[k]))
                {
                    //It then goes through each of the names that it will eat
                    foreach (string P in Prey)
                    {
                        //It will then compare that to the current animal it is looking at 
                        if (R.CompareTag(P) && DangerLvl >= R.GetComponent<Generalist_AI>().DangerLvl)
                        {
                            //If it is the animal and the danger lvl isnt too high itll get how far away it is
                            float dist = Vector3.Distance(R.transform.position, currentPos);
                            //Itll then pick the one closest and set it to minDist
                            if (dist < minDist)
                            {
                                tMin = R.transform;
                                minDist = dist;
                            }
                        }
                        //This runs the same for the starving targets aswell
                        else if (Food <= 300)
                        {
                            foreach (string S in StarvingPrey)
                            {
                                if (R.CompareTag(S) && (DangerLvl*2) >= R.GetComponent<Generalist_AI>().DangerLvl)
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
        //Return the closest object
        return tMin;
    }

    //This does the same as the ClosestFoodRenderers just for water
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
        //If it cant find one then move and try again
        if (Pos == OldPos)
        {
            Pos = CalculateNextPos();
        }
        return Pos;
    }
    //This is where the eyes check if it can see it 
    //Not my code. Found at: 
    private bool IsVisible(Renderer renderer, Camera Eye)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Eye);
        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            return true;
        else
            return false;
    }

    //This is the CheckSurroundings function that is used by all animals
    protected bool CheckSurroundings(CurrentAction CurAct)
    {
        //If its thirsty itll look for the water and see if its close enough to drink
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
        //If its hungry and a herbivore itll look for the water and see if its close enough to eat
        if (CurAct == CurrentAction.GRAZING)
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
