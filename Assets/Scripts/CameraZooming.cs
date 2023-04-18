using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZooming : MonoBehaviour
{
    public int[] Zoom;
    public int[] PosLimit;
    public Camera camera;
    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            float Test = camera.orthographicSize += Input.mouseScrollDelta.y * 10;
            Debug.Log(Test);
            if (Test >= Zoom[1])
            {
                camera.orthographicSize = Zoom[1];
            }
            else if (Test <= Zoom[0])
            {
                camera.orthographicSize = Zoom[0];
            }
            else
                camera.orthographicSize += Input.mouseScrollDelta.y * 10;
        }
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Vector3 Pos = new(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
            if(camera.transform.position.x += Pos.x <= PosLimit[0])
            {
                camera.transform.position = new()
            }
            camera.transform.position = Pos;
        }
    }
}
