using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
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
        if(Input.GetKeyDown(KeyCode.F) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            Debug.Log("Item added to inventory");
            
            if(!InventorySystem.Instance.CheckIfFull()) 
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
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    //Player out of range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
