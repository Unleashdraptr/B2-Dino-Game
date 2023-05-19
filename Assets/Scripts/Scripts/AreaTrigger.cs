using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public int QuestID;
    bool InArea;
    public float Timer;
    private void Update()
    {
        if(InArea)
        {
            Timer -= 1 * Time.deltaTime;
        }
        if(Timer <= 0)
        {
            GameObject.Find("Player UI").GetComponent<QuestStorage>().FinishQuest(QuestID);  
            Destroy(gameObject);
        }
    }
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
