using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd_AI : MonoBehaviour
{
    //Exclusive use of Herbivores/Omnivores
    float radius;
    Vector3 Centre;
    // Start is called before the first frame update
    void Start()
    {
        radius = transform.childCount * 10;
    }

    // Update is called once per frame
    void CalculateBoundries()
    {
        int Exp = Random.Range(1, transform.childCount);
        Centre = transform.GetChild(Exp - 1).transform.position;
    }
}
