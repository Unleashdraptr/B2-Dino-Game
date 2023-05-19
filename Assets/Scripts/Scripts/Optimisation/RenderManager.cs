using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RenderManager : MonoBehaviour
{
    //Rendering Optimisation
    public GameObject[] TreeLocations;
    public GameObject Terrain;
    public GameObject Animals;
    public GameObject Player;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 PlayerMoveInput = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Checks if the player is moving and if not to go to Idle
        if (!PlayerMoveInput.Equals(default))
        {
            UpdateTerrainChunks();
            UpdateTrees();
            UpdateAnimals();
        }
    }
    public void UpdateTerrainChunks()
    {
        for (int i = 0; i < Terrain.transform.childCount; i++)
        {
            if (Vector3.Distance(Terrain.transform.GetChild(i).position, Player.transform.position) < 850)
            {
                Terrain.transform.GetChild(i).GetComponent<Terrain>().enabled = true;
            }
            else if (Vector3.Distance(Terrain.transform.GetChild(i).position, Player.transform.position) > 2500)
            {
                Terrain.transform.GetChild(i).GetComponent<Terrain>().enabled = false;
            }
            else
            {
                if (IsVisible(Terrain.transform.GetChild(i).GetComponent<MeshRenderer>(), GameObject.Find("Main Camera").GetComponent<Camera>()))
                {
                    Terrain.transform.GetChild(i).GetComponent<Terrain>().enabled = true;
                }
                else
                    Terrain.transform.GetChild(i).GetComponent<Terrain>().enabled = false;
            }
        }
    }
    void UpdateAnimals()
    {
        for (int i = 1; i < TreeLocations.Length; i++)
        {
            if (Vector3.Distance(Animals.transform.GetChild(i).transform.position, Player.transform.position) < 250)
            {
                Animals.transform.GetChild(i).gameObject.SetActive(true);
                Animals.transform.GetChild(i).GetComponent<NavMeshAgent>().enabled = true;
            }
            else if (Vector3.Distance(Animals.transform.GetChild(i).transform.position, Player.transform.position) > 250)
            {
                Animals.transform.GetChild(i).gameObject.SetActive(false);
                Animals.transform.GetChild(i).GetComponent<NavMeshAgent>().enabled = false;
            }
        }
    }
    void UpdateTrees()
    {
        for (int i = 0; i < TreeLocations.Length; i++)
        {
            if (Vector3.Distance(TreeLocations[i].transform.position, Player.transform.position) < 1000)
            {
                TreeLocations[i].SetActive(true);
            }
            else if (Vector3.Distance(TreeLocations[i].transform.position, Player.transform.position) > 1000)
            {
                TreeLocations[i].SetActive(false);
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
