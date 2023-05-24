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
        //If the animal is alive itll move
        if (CurAct != CurrentAction.DEAD)
        {
            //Itll slowly starve and need water
            Food -= 5 * Time.deltaTime;
            thirst -= 5 * Time.deltaTime;
            //Itll monitor these levels and change what its doing accordingly
            CheckState(CurAct, ThirtinessLvl, StarvationLvl);
            UpdateStates(CurAct, ThirtinessLvl, StarvationLvl, DangerLvl);
            Move.destination = NextMovement(Habitat);
        }
        else
            Move.speed = 0;
    }
    //Gets a positon that would be along the lake that it currently stays by, will be changed out later
    Vector3 NextMovement(Transform Habitat)
    {
        float x = Random.Range(Habitat.position.x - (Habitat.lossyScale.x * 6), Habitat.position.x + (Habitat.lossyScale.x * 6));
        float z = Random.Range(Habitat.position.z - (Habitat.lossyScale.z * 6), Habitat.position.z + (Habitat.lossyScale.z * 6));
        Vector3 Pos = new(x, Habitat.position.y, z);
        CurAct = CurrentAction.MOVING;
        return Pos;
    }
    //This is how it finds the closest water
    Transform FindClosestWaterHabitat()
    {
        //It sets the closest the furthest away it can at the beginning and then sets its own position
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        //Using both eyes (Cameras)
        for (int k = 0; k < Eyes.Length; k++)
        {
            //Checks each Renderer in the terrain Object (Each chunk it can see)
            foreach (Renderer R in Locations)
            {
                //It gets its distance from the Terrain it can see
                float dist = Vector3.Distance(R.transform.position, currentPos);
                //if its closer than the current one then replae it 
                if (dist < minDist)
                {
                    tMin = R.transform;
                    minDist = dist;
                }
            }
        }
        //Once its done then itll head to what is the closest object
        return tMin;
    }
}
