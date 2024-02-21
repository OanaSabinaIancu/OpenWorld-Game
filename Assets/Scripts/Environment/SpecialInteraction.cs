using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is use to interact with object that can not be picked up
public class SpecialInteraction : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.F) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            Debug.Log("Crafting bench open");
        }
    }

    //Player enter in range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
