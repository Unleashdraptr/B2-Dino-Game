using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WaypointManager : MonoBehaviour
{
    public GameObject[] WaypointIcons;
    public MeshRenderer[] Waypoints;
    public GameObject WaypointModifer;

    int Mult = 1;
    int MultNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        Waypoints = transform.GetComponentsInChildren<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (UIManager.InMap == true)
        {
            UpdateWaypointPage();
        }
        /*
        if (Input.GetMouseButton(0) && CreatingWaypoint == true)
        {
            MousePos = WaypointCamera.ScreenToWorldPoint(Input.mousePosition);
            MousePos.x = (MousePos.x / 5) * 4;
            MousePos.z = (MousePos.z / 3) * 4;
            Instantiate(test, MousePos, Quaternion.identity);
        }
        */
    }
    public void WaypointHoverEnter(int WaypointNum)
    {
        Waypoints[MultNum + WaypointNum].GetComponent<Animator>().SetBool("IsHoveredOn", true);
    }
    public void WaypointHoverExit(int WaypointNum)
    {
        Waypoints[MultNum + WaypointNum].GetComponent<Animator>().SetBool("IsHoveredOn", false);
    }
    public void NextPage()
    {
        if (WaypointIcons[WaypointIcons.Length - 1].activeInHierarchy)
        {
            Mult += 1;
            for (int i = 0; i < WaypointIcons.Length; i++)
            {
                WaypointIcons[i].SetActive(false);
            }
            WaypointModifer.SetActive(false);
            UpdateWaypointPage();
        }
    }
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
        WaypointModifer.SetActive(false);
        UpdateWaypointPage();
    }
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
