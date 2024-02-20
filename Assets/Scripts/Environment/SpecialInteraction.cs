using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is use to interact with object that can not be picked up
public class SpecialInteraction : MonoBehaviour
{
    public static SpecialInteraction Instance { get; set; }

    // Start is called before the first frame update
    public string ItemName;

    // Check if player is in range of on object 
    public bool playerInRange;

    public string GetItemName()
    {
        return ItemName;
    }

    //Pick up the items
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            Debug.Log("Crafting bench open");

            if (!InventorySystem.Instance.CheckIfFull())
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                Destroy(gameObject);

            }
            else
            {
                Debug.Log("The inventory is full");
            }
        }
    }

    //Player enter in range
    public bool OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        return playerInRange;
    }

    //Player out of range
    public bool OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
        return playerInRange;
    }
}
