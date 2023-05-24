using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pack_AI : MonoBehaviour
{
    //Exclusive Use on Carnivore/Omnivore AI
    public GameObject Alpha;
    public Transform AlphaHuntTarget;
    public Vector3 AlphaWalkLocation;

    //The combined danger level used by the Pack for attacking
    public int CombDangerLvl;
    //The combined Packaction based off the pack needs
    public Generalist_AI.CurrentAction PackAction;
    public Generalist_AI.CurrentAction[] PackNeeds;
    public int[] CurStates;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the Packneeds to how many members there are
        PackNeeds = new Generalist_AI.CurrentAction[transform.childCount - 1];
        //It will then decide the Alpha randomly and tell that pack member its alpha
        int AlphaNum = Random.Range(1, transform.childCount);
        Alpha = transform.GetChild(AlphaNum).gameObject;
        Alpha.GetComponent<Carnivourous_AI>().IsAlpha = true;
        //It will then loop through all the members to get the Combined Danger level and set their Pack number
        for(int i = 1; i < transform.childCount; i++)
        {
            CombDangerLvl += transform.GetChild(i).GetComponent<Generalist_AI>().DangerLvl;
            transform.GetChild(i).GetComponent<Carnivourous_AI>().PackNum = i-1;
        }
    }
    //The alpha will run this when they need something to see what everyone else needs
    public void FindLargestNeed()
    {
        //It runs through every pack member to tally what Action all of them are doing at that moment
        CurStates = new int[6];
        for (int i = 0; i < PackNeeds.Length; i++)
        {
            switch (PackNeeds[i])
            {
                case Generalist_AI.CurrentAction.HUNGRY:
                    CurStates[0] += 1;
                    break;
                case Generalist_AI.CurrentAction.WATERING:
                    CurStates[1] += 1;
                    break;
                case Generalist_AI.CurrentAction.MOVING:
                    CurStates[2] += 1;
                    break;
                case Generalist_AI.CurrentAction.IDLE:
                    CurStates[3] += 1;
                    break;
                case Generalist_AI.CurrentAction.HUNTING:
                    CurStates[4] += 1;
                    break;
                case Generalist_AI.CurrentAction.FEEDING:
                    CurStates[5] += 1;
                    break;
            }
        }
        //If 1 action has the majority, the alpha will then perform that action
        int Maxi = CurStates.Max();
        if (Maxi == CurStates[0])
        {
            PackAction = Generalist_AI.CurrentAction.HUNGRY;
        }
        if (Maxi == CurStates[1])
        {
            PackAction = Generalist_AI.CurrentAction.WATERING;
        }
        if (Maxi == CurStates[2])
        {
            PackAction = Generalist_AI.CurrentAction.MOVING;
        }
        if (Maxi == CurStates[3])
        {
            PackAction = Generalist_AI.CurrentAction.IDLE;
        }
        if (Maxi == CurStates[4])
        {
            PackAction = Generalist_AI.CurrentAction.HUNTING;
        }
        if (Maxi == CurStates[5])
        {
            PackAction = Generalist_AI.CurrentAction.FEEDING;
        }
    }
}
