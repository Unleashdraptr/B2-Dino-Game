using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This was how the player could move around the map and zoom in
public class CameraZooming : MonoBehaviour
{
    //The Zoom contains the Min and the Max
    public int[] Zoom;
    //The camera it applied to
    public Camera camera;
    //The min and max distances with the scaling
    float MaxInt;
    float MinInt;
    float MaxScale;
    float MinScale;
    // Update is called once per frame
    void Update()
    {
        //If we are in the map then we can change this
        if (UIManager.InMap == true)
        {
            //We use these math equations to find the borders for the sizes we need
            MinScale = 10 - ((camera.orthographicSize - 500) / 2222.5f) * 10;
            MaxScale = 2 - camera.orthographicSize / 2500;
            MaxInt = Zoom[1] * MaxScale;
            MinInt = Zoom[1] / MinScale;
            //If we use the scroll wheel then the Max & Min Int will change
            if (Input.mouseScrollDelta.y != 0)
            {
                //It will then update its zoom level to still be within the borders 
                float CamZoom = camera.orthographicSize -= Input.mouseScrollDelta.y * 10;
                if (CamZoom >= Zoom[1])
                {
                    camera.orthographicSize = Zoom[1];
                }
                else if (CamZoom <= Zoom[0])
                {
                    camera.orthographicSize = Zoom[0];
                }
                else
                    camera.orthographicSize -= Input.mouseScrollDelta.y * 10;
                //This checks if it is within the borders and set it as such if it isnt
                if (camera.transform.position.x <= MinInt)
                {
                    camera.transform.position = new(MinInt, camera.transform.position.y, camera.transform.position.z);
                }
                else if (camera.transform.position.x >= MaxInt)
                {
                    camera.transform.position = new(MaxInt, camera.transform.position.y, camera.transform.position.z);
                }
                if (camera.transform.position.z <= MinInt)
                {
                    camera.transform.position = new(camera.transform.position.x, camera.transform.position.y, MinInt);
                }
                else if (camera.transform.position.z >= MaxInt)
                {
                    camera.transform.position = new(camera.transform.position.x, camera.transform.position.y, MinInt);
                }
            }
            //We then use the mouse position changes when the player clicks to move around
            if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && Input.GetMouseButton(0))
            {
                //It also checks if its within the limits and will set it to it if its not
                Vector3 Pos = new(camera.transform.position.x - Input.GetAxis("Mouse X") * 25, camera.transform.position.y, camera.transform.position.z - Input.GetAxis("Mouse Y") * 25);
                if (Pos.x <= MinInt)
                {
                    Pos.x = MinInt;
                }
                else if (Pos.x >= MaxInt)
                {
                    Pos.x = MaxInt;
                }
                if (Pos.z <= MinInt)
                {
                    Pos.z = MinInt;
                }
                else if (Pos.z >= MaxInt)
                {
                    Pos.z = MaxInt;
                }
                else
                    camera.transform.position = Pos;
            }
        }
    }
}
