using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    public GameObject numberHolder;

    public GameObject selectedItem;
    public int selectedNumber;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        PopulateSlotList();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SelectQuickSlot(0);
        }
    }

    void SelectQuickSlot(int number)
    {
        if(checkIfSlotIsFull(number))
        {
            if(selectedNumber != number)
            {
                selectedNumber = number;
                selectedItem = getSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                //deselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                //color
                Text toBeChanged = numberHolder.transform.Find("number").transform.Find("Text").GetComponent<Text>();
                toBeChanged.color = Color.blue;
            }
            else
            {
                selectedNumber = -1;
                if(selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
            }
        }
    }

    GameObject getSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber].transform.GetChild(0).gameObject;
    }

    bool checkIfSlotIsFull(int number)
    {
        if (quickSlotsList[0].transform.childCount > 0)
            return true;
        return  false;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemList.Add(cleanName);
    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
