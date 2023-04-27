using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spinosauridae_AI : Carnivourous_AI
{
    //Since this is an semi-aquatic creature, itll spend most if not all its time in the water
    //Itll store all the water locations in this WaterLocations
    public Transform WaterLocations;
    //Habitat is where its currently water habitat will be 
    Transform Habitat;
    //Itll use this to spot all the areas it can use as a habitat
    Renderer[] Locations;

    public float ThirtinessLvl;
    public float StarvationLvl;

    //The AI starts by getting its NavMesh agent from the gameObject
    void Start()
    {
        //Gets the renders for all the water locations
        Locations = WaterLocations.GetComponentsInChildren<Renderer>();
        Move = GetComponent<NavMeshAgent>();
        //Sets up its food, thirst and health
        Food = 1000;
        thirst = 1000;
        Hp = 100;
        //Grabs the other objects that are needed for general functioning
        AllObjectsNeeded();
        //This will find the closest water supply
        Habitat = FindClosestWaterHabitat();
        //It them sets the first movement destination, this will be changed soon
        Move.destination = NextMovement(Habitat);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurAct != CurrentAction.DEAD)
        {
            Food -= 5 * Time.deltaTime;
            thirst -= 5 * Time.deltaTime;
            CheckState(CurAct, ThirtinessLvl, StarvationLvl);
            UpdateStates(ThirtinessLvl, StarvationLvl);
            Move.destination = NextMovement(Habitat);
        }
        else
            Move.speed = 0;
    }
    Vector3 NextMovement(Transform Habitat)
    {
        float x = Random.Range(Habitat.position.x - (Habitat.lossyScale.x * 6), Habitat.position.x + (Habitat.lossyScale.x * 6));
        float z = Random.Range(Habitat.position.z - (Habitat.lossyScale.z * 6), Habitat.position.z + (Habitat.lossyScale.z * 6));
        Vector3 Pos = new(x, Habitat.position.y, z);
        CurAct = CurrentAction.MOVING;
        return Pos;
    }
    Transform FindClosestWaterHabitat()
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        for (int k = 0; k < Eyes.Length; k++)
        {
            foreach (Renderer R in Locations)
            {
                float dist = Vector3.Distance(R.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = R.transform;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }
}
