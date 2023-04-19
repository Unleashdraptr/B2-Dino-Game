using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZooming : MonoBehaviour
{
    public int[] Zoom;
    public Camera camera;
    float MaxInt;
    float MinInt;
    float MaxScale;
    float MinScale;
    // Update is called once per frame
    void Update()
    {
        if (UIManager.InMap == true)
        {
            MinScale = 10 - ((camera.orthographicSize - 500) / 2222.5f) * 10;
            MaxScale = 2 - camera.orthographicSize / 2500;
            MaxInt = Zoom[1] * MaxScale;
            MinInt = Zoom[1] / MinScale;
            if (Input.mouseScrollDelta.y != 0)
            {
                float Test = camera.orthographicSize -= Input.mouseScrollDelta.y * 10;
                if (Test >= Zoom[1])
                {
                    camera.orthographicSize = Zoom[1];
                }
                else if (Test <= Zoom[0])
                {
                    camera.orthographicSize = Zoom[0];
                }
                else
                    camera.orthographicSize -= Input.mouseScrollDelta.y * 10;
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
            if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && Input.GetMouseButton(0))
            {
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
