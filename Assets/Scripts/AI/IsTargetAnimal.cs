using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetAnimal : MonoBehaviour
{
    Generalist_AI AI;
    // Start is called before the first frame update
    private void Start()
    {
        AI = GetComponent<Generalist_AI>();
    }
    void Awake()
    {
        AI.DontDespawn = true;
    }
}
