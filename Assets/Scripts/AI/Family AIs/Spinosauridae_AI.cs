using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spinosauridae_AI : Carnivourous_AI
{
    public Transform WaterLocations;
    Transform Habitat;
    Renderer[] Locations;
    void Start()
    {
        Locations = WaterLocations.GetComponentsInChildren<Renderer>();
        Move = GetComponent<NavMeshAgent>();
        Food = 1000;
        thirst = 1000;
        Move.destination = CalculateNextPos();
        AllObjectsNeeded();
        Habitat = FindClosestWaterHabitat();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurAct != CurrentAction.DEAD)
        {
            Food -= 5 * Time.deltaTime;
            thirst -= 5 * Time.deltaTime;
            if (CurAct == CurrentAction.NEXTMOVEMENT)
            {
                Debug.Log("VRN-bvnW=");
                Move.destination = NextMovement(Habitat);
            }
            CheckState();
            UpdateStates();
        }
        else
            Move.speed = 0;
    }
    Vector3 NextMovement(Transform Habitat)
    {
        float x = Random.Range(Habitat.position.x - (Habitat.position.x / 2), Habitat.position.x + (Habitat.position.x / 2));
        float z = Random.Range(Habitat.position.z - (Habitat.position.z / 2), Habitat.position.z + (Habitat.position.z / 2));
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
