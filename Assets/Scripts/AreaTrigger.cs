using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is used by the Area quest to tell when the player is exploring the area specified
public class AreaTrigger : MonoBehaviour
{
    //It stores the questID its related to
    public int QuestID;
    //The check for if the player is in the area
    bool InArea;
    //The amount of time they have to spend in the area
    public float Timer;
    private void Update()
    {
        //If they are in the area then countdown
        if(InArea)
        {
            Timer -= 1 * Time.deltaTime;
        }
        //If it reaches 0 then remove the waypoint and reward the player
        if(Timer <= 0)
        {
            GameObject.Find("Player UI").GetComponent<QuestStorage>().FinishQuest(QuestID);  
            Destroy(gameObject);
        }
    }
    //Checks to see if the player is currently colliding with the area they need to be in 
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            InArea = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InArea = false;
        }
    }
}
