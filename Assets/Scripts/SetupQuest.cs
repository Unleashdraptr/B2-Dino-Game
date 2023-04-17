using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupQuest : MonoBehaviour
{
    public GameObject[] AnimalsToSpawn;
    public Transform[] WorldLocation;
    int[] RowLength;
    //Hunter's faction
    public void TargetAnimal(int AnimalID, Transform LocationOfArea)
    {
        float x = LocationOfArea.position.x + Random.Range(-500, 500);
        float z = LocationOfArea.position.z + Random.Range(-500, 500);
        Vector3 Pos = new(x, WorldLocation[FindChunkNum(x, z)].position.y, z);
        GameObject Target = Instantiate(AnimalsToSpawn[AnimalID], Pos, Quaternion.identity, GameObject.Find("AnimalStorage").transform);
        Target.GetComponent<IsTargetAnimal>().enabled = true;
    }
    public void SpawnLostItem(GameObject Item, Transform LocationOfArea)
    {

    }
    int FindChunkNum(float x, float y)
    {
        int ChildNum = 0;
        int XOffset = (int)(x / 312.5)-1;
        int YOffset = (int)(y / 312.5)-1;
        for(int i =0; i < XOffset; i++)
        {
            ChildNum += RowLength[i];
        }
        ChildNum += YOffset;
        return ChildNum;
    }
}
