using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is for when the animal is a quest animal, Specifically for the Bounty
public class IsTargetAnimal : MonoBehaviour
{
    Generalist_AI AI;
    public GameObject Waypoint;
    void Awake()
    {
        //It makes them larger and more powerful and makes usre that they cant despawn
        AI = GetComponent<Generalist_AI>();
        AI.DontDespawn = true;
        Waypoint.SetActive(true);
        AI.Hp *= 1.5f;
        transform.localScale = new(1.5f, 1.5f, 1.5f);
    }
}
