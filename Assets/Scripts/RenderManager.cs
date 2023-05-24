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

    //This checks if the Terrain, trees and animals are in range of the player and will turn them on or off
    //depending on the distance
    void FixedUpdate()
    {
        Vector3 PlayerMoveInput = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Checks if the player is moving and if not to go to Idle
        if (!PlayerMoveInput.Equals(default))
        {
            //Each of these functions check their respective objects
            UpdateTerrainChunks();
            UpdateTrees();
            UpdateAnimals();
        }
    }
    public void UpdateTerrainChunks()
    {
        //Itll loop through each of the Terrain chunks and check if its too far away to turn on
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
            //If its inbetween these 2 distances then use the camera to check if it should render
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
        //Itll loop through each of the animals and check if its too far away to turn on
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
        //Itll loop through each of the tree group locations and check if its too far away to get on
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
    //This is where the eyes check if it can see it 
    //Not my code. Found at: 
    private bool IsVisible(Renderer renderer, Camera Eye)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Eye);
        if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            return true;
        else
            return false;
    }
}
