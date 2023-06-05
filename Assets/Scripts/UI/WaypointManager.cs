using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This is used by the map UI to name the waypoints
public class WaypointManager : MonoBehaviour
{
    //This will set all the waypoints and get their information
    public GameObject[] WaypointIcons;
    public MeshRenderer[] Waypoints;

    int Mult = 1;
    int MultNum = 0;
    // Start is called for all the waypoints
    void Start()
    {
        Waypoints = transform.GetComponentsInChildren<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        //Update the waypoints if in the map UI
        if (UIManager.InMap == true)
        {
            UpdateWaypointPage();
        }
        /*
        //Unused
        if (Input.GetMouseButton(0) && CreatingWaypoint == true)
        {
            MousePos = WaypointCamera.ScreenToWorldPoint(Input.mousePosition);
            MousePos.x = (MousePos.x / 5) * 4;
            MousePos.z = (MousePos.z / 3) * 4;
            Instantiate(test, MousePos, Quaternion.identity);
        }
        */
    }

    //This will cause the Waypoint to move if they hover over the name connected to it
    public void WaypointHoverEnter(int WaypointNum)
    {
        Waypoints[MultNum + WaypointNum].GetComponent<Animator>().SetBool("IsHoveredOn", true);
    }
    public void WaypointHoverExit(int WaypointNum)
    {
        Waypoints[MultNum + WaypointNum].GetComponent<Animator>().SetBool("IsHoveredOn", false);
    }
    //This will then flip to the next page of these waypoints
    public void NextPage()
    {
        if (WaypointIcons[WaypointIcons.Length - 1].activeInHierarchy)
        {
            Mult += 1;
            for (int i = 0; i < WaypointIcons.Length; i++)
            {
                WaypointIcons[i].SetActive(false);
            }
            UpdateWaypointPage();
        }
    }
    //This will then flip to the previous page of these waypoints
    public void PrevPage()
    {
        Mult -= 1;
        if(Mult < 1)
        {
            Mult = 1;
        }
        for (int i = 0; i < WaypointIcons.Length; i++)
        {
            WaypointIcons[i].SetActive(false);
        }
        UpdateWaypointPage();
    }
    //This will then loop through the available slots and then assign the waypoints to their locations
    void UpdateWaypointPage()
    {
        for (int i = 1; i < Mult; i++)
        {
            MultNum += 5;
        }
        for (int i = 0; i < 6; i++)
        {
            if (i < (Waypoints.Length - MultNum))
            {
                WaypointIcons[i].SetActive(true);
                WaypointIcons[i].GetComponent<TextMeshProUGUI>().text = Waypoints[MultNum + i].name;
                WaypointIcons[i].transform.GetChild(0).GetComponent<Image>().material = Waypoints[MultNum + i].material;
                //If it runs out of slots then stop
                if (i >= WaypointIcons.Length - 1)
                {
                    break;
                }
            }
            /*
            else
                SpawnCreateWaypoint(i);
            */
        }
        MultNum = 0;
    }
    /*
    //Unused
    void SpawnCreateWaypoint(int StartPos)
    {
        WaypointModifer.SetActive(true);
        WaypointModifer.transform.position = new(WaypointModifer.transform.position.x, WaypointIcons[StartPos].transform.position.y, WaypointModifer.transform.position.z);
    }
    public void WaypointCreate()
    {
        CreatingWaypoint = true;
    }
    */
}
