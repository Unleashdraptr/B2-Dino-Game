using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pack_AI : MonoBehaviour
{
    //Exclusive Use on Carnivore/Omnivore AI
    public GameObject Alpha;
    public Transform AlphaHuntTarget;
    public Vector3 AlphaWalkLocation;
    public float radius;
    public int CombDangerLvl;
    public Generalist_AI.CurrentAction PackAction;
    public Generalist_AI.CurrentAction[] PackNeeds;

    // Start is called before the first frame update
    void Start()
    {
        PackNeeds = new Generalist_AI.CurrentAction[transform.childCount - 1];
        int AlphaNum = Random.Range(1, transform.childCount);
        Alpha = transform.GetChild(AlphaNum).gameObject;
        Alpha.GetComponent<Carnivourous_AI>().IsAlpha = true;
        for(int i = 1; i < transform.childCount; i++)
        {
            CombDangerLvl += transform.GetChild(i).GetComponent<Generalist_AI>().DangerLvl;
            transform.GetChild(i).GetComponent<Carnivourous_AI>().PackNum = i-1;
        }
    }
}
