using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Transform ItemPickUpStorage;
    public Transform WorldItems;
    public GameObject PickUpPrompt;
    public Camera CameraRayCast;
    public bool holdingItem;

    private void Update()
    {
        Ray Point = CameraRayCast.ScreenPointToRay(new(Screen.width/2, Screen.height/2,0));
        if (Physics.Raycast(Point, out RaycastHit hit))
        {
            Debug.Log(Vector3.Distance(hit.transform.position, transform.position));
            if(hit.transform.CompareTag("Item") && holdingItem == false && Vector3.Distance(hit.transform.position, transform.position) < 5)
            {
                PickUpPrompt.SetActive(true);
                if (PickUpPrompt.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(hit.transform);
                }
            }
            else if(holdingItem == true)
            {
                PickUpPrompt.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    DropItem();
                }
            }
            else
                PickUpPrompt.SetActive(false);
        }
    }
    public void PickUpItem(Transform Item)
    {
        if(Item.GetComponent<ItemID>().isPickUpable)
        {
            Item.SetParent(ItemPickUpStorage);
            Item.localPosition = new(0, 0, 0);
            holdingItem = true;
        }
    }
    public void DropItem()
    {
        Ray Point = CameraRayCast.ScreenPointToRay(new(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(Point, out RaycastHit hit))
        {
            Vector3 NewPos = hit.point;
            if(Vector3.Distance(hit.transform.position, transform.position) > 10)
            {
                NewPos = new(10, hit.transform.position.y * 2, 10);
            }
            ItemPickUpStorage.GetChild(0).SetPositionAndRotation(NewPos, ItemPickUpStorage.GetChild(0).rotation);
            ItemPickUpStorage.GetChild(0).SetParent(WorldItems);
            holdingItem = false;
        }
    }
}
