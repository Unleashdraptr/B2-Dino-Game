using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetAnimal : MonoBehaviour
{
    Generalist_AI AI;
    public GameObject Waypoint;
    void Awake()
    {
        AI = GetComponent<Generalist_AI>();
        AI.DontDespawn = true;
        Waypoint.SetActive(true);
        AI.Hp *= 1.5f;
        transform.localScale = new(1.5f, 1.5f, 1.5f);
    }
}
