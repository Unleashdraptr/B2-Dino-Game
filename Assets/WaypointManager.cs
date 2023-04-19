using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public GameObject[] WaypointIcons;
    GameObject[] Waypoints;
    public GameObject WaypointCreate;
    public GameObject WaypointRemove;
    // Start is called before the first frame update
    void Start()
    {
        Waypoints = transform.GetComponentsInChildren<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WaypointHoverEnter(int WaypointNum)
    {
        Waypoints[WaypointNum].GetComponent<Animator>().SetBool("IsHoveredOn", true);
    }
    public void WaypointHoverExit(int WaypointNum)
    {
        Waypoints[WaypointNum].GetComponent<Animator>().SetBool("IsHoveredOn", false);
    }
}
