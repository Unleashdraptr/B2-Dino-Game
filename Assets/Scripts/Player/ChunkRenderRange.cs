using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderRange : MonoBehaviour
{
    public Camera camera;
    public GameObject Terrain;
    public void UpdateTerrainChunks()
    {
        for(int i = 0; i < Terrain.transform.childCount; i++)
        {
            if(IsVisible(Terrain.transform.GetChild(i).GetComponent<Terrain>().GetComponent<MeshRenderer>(), camera))
            {
                Terrain.transform.GetChild(i).GetComponent<Terrain>().enabled = true;
            }
            else
                Terrain.transform.GetChild(i).GetComponent<Terrain>().enabled = false;
            if (Vector3.Distance(Terrain.transform.GetChild(i).position, transform.position) < 500)
            {
                Terrain.transform.GetChild(i).GetComponent<Terrain>().enabled = true;
            }
        }
    }
    private bool IsVisible(Renderer renderer, Camera Eye)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Eye);
        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            return true;
        else
            return false;
    }
}
