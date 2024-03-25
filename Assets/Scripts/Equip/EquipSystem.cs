using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; private set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;
    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<GameObject> inventoryItems = new List<GameObject>(); // List to store inventory items

    public GameObject numberHolder;
    public GameObject selectedItem;
    public int selectedNumber;

    public GameObject toolHolder;

    public GameObject selectedItemModel;

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

        // Instantiate the sword model from resources
        GameObject swordItem = Instantiate(Resources.Load<GameObject>("LongSword_Model"));
        string SwordItemName = swordItem.name.Replace("(Clone)","");
        swordItem.name = SwordItemName;

        Debug.Log(swordItem.name);

        if (swordItem != null)
        {
            // Replace the name with the desired name
            swordItem.name = "LongSword_Model";

            // Add the sword to the player's back
            SetEquippedModel(swordItem);
        }
        else
        {
            Debug.LogError("Failed to load LongSword_Model from resources.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SelectQuickSlot(0);
        }

        UpdateItemInfoDescription();
    }

    void UpdateItemInfoDescription()
    {
        if (selectedItem != null)
        {
            InventoryItem selectedItemInventoryItem = selectedItem.GetComponent<InventoryItem>();
            if (selectedItemInventoryItem != null)
            {
                string itemName = selectedItemInventoryItem.thisName;
                int itemPosition = InventorySystem.Instance.itemList.IndexOf(itemName);
                int quantity = InventorySystem.Instance.itemCount[itemPosition];
                if (quantity > 0)
                {
                    selectedItemInventoryItem.itemInfoUI_itemDescription.text = "Object consumed. Quantity remains " + quantity;
                }
            }
        }
    }

    void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number))
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;

                // Deselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = getSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                //SetEquippedModel(selectedItem);

                int itemPosition = InventorySystem.Instance.itemList.IndexOf(selectedItem.name);

                //Check if we have a only one item into the slot and delete the item from the quick slot
                if (InventorySystem.Instance.itemCount[itemPosition] >= 1 && Input.GetKeyDown(KeyCode.Z)) 
                { 
                    selectedItemModel = selectedItem.gameObject;
                }

                if(selectedItemModel != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                // Change color
                Text toBeChanged = numberHolder.transform.Find("number").transform.Find("Text").GetComponent<Text>();
                toBeChanged.color = Color.blue;
            }
            else
            {
                int itemPosition = InventorySystem.Instance.itemList.IndexOf(selectedItem.name);

                selectedNumber = -1;

                //DeselectHandler previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                //Check if we have a only one item into the slot and delete the item from the quick slot
                if (InventorySystem.Instance.itemCount[itemPosition] >= 1 && Input.GetKeyDown(KeyCode.Z))
                {
                    selectedItemModel = selectedItem.gameObject;
                }

                if (selectedItemModel != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }
            }
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        //string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        //GameObject itemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(0.6f, 0, 0.4f), Quaternion.Euler(0,-12.5f, -1));
        //GameObject itemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"));

        if (selectedItemModel != null)
        {
            selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
            selectedItem = null;
        }

        selectedItemModel = selectedItem;

        // Adjust scale of the object to make it bigger
        selectedItem.transform.localScale = new Vector3(2f, 2f, 2f); // Increase size by 2 times

        //itemModel.transform.SetParent(toolHolder.transform, false);

        // Adjust position and rotation
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
        selectedItemModel.transform.localPosition = new Vector3(0.6f, 0, 0.4f); // Position on the player's back
        selectedItemModel.transform.localRotation = Quaternion.Euler(210, 0, 0); // Rotate upside down
    }

    GameObject getSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber].transform.GetChild(0).gameObject;
    }

    bool checkIfSlotIsFull(int number)
    {
        return quickSlotsList[0].transform.childCount > 0;
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
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");

        // Update isEquipped flag of the InventoryItem
        InventoryItem inventoryItem = itemToEquip.GetComponent<InventoryItem>();

        if (inventoryItem != null)
        {
            inventoryItem.isSelected = true;
        }

        // Check if the item is already in the inventory
        if (!inventoryItems.Contains(itemToEquip))
        {
            // Add the item to the inventory list if its quantity is greater than 0
            int itemPos = InventorySystem.Instance.itemList.IndexOf(cleanName);
            if (inventoryItem != null && itemPos != -1 && InventorySystem.Instance.itemCount[itemPos] > 0)
            {
                inventoryItems.Add(itemToEquip);
            }
        }

        // Find next free slot in the quick slots panel
        GameObject availableSlot = FindNextEmptySlot();

        // Instantiate a copy of the item
        GameObject itemCopy = Instantiate(itemToEquip, availableSlot.transform);

        // Set transform of the object
        itemCopy.transform.localPosition = Vector3.zero;
        itemCopy.transform.localScale = Vector3.one;

        // Add the copy to the quick slots list
        quickSlotsList.Add(itemCopy);

        // Check if the item quantity is less than or equal to 0
        int itemPosition = InventorySystem.Instance.itemList.IndexOf(cleanName);
        if (itemPosition != -1 && InventorySystem.Instance.itemCount[itemPosition] <= 0)
        {
            // Update item description
            InventoryItem copyInventoryItem = itemCopy.GetComponent<InventoryItem>();
            if (copyInventoryItem != null)
            {
                copyInventoryItem.itemInfoUI_itemDescription.text = "No more items left.";
            }

            // If the quantity is 0 or less, remove the item from the quick slots list
            quickSlotsList.Remove(itemCopy);
            // Also remove from inventory items list
            inventoryItems.Remove(itemToEquip);

            // Destroy the copy of the item in the quick slot
            Destroy(itemCopy);
        }
        else
        {
            // If the quantity is greater than 0, update the item description
            if (inventoryItem != null)
            {
                inventoryItem.itemInfoUI_itemDescription.text = "Object consumed. Quantity remains " + InventorySystem.Instance.itemCount[itemPosition];
            }
        }
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

        return counter == 1;
    }
}
