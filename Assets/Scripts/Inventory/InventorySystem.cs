using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    //reference for the item info
    public GameObject ItemInfoUI;

    public GameObject inventoryScreenUI;
    public bool isOpen;

    //declarations for object list
    public List<GameObject> slotList = new List<GameObject>(); //list of slots
    public List<string> itemList = new List<string>(); //list of object names
    public List<int> itemCount = new List<int>(); //list of quantity
    public int maxQuantity = 10;

    public GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public GameObject pickUpAlert;
    public Text pickupName;
    public Image pickupImage;


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

    void Start()
    {
        isOpen = false;
        ItemInfoUI.SetActive(false);
        PopulateSlotList();

        Cursor.visible = false;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B) && !isOpen)
        {

            Debug.Log("b is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            PlayerMovement.Instance.controller.enabled = false;
            isOpen = true;

            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        }
        else if ((Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape)) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            //blocking character's movement
            PlayerMovement.Instance.controller.enabled = true;
            isOpen = false;

            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }

    public void AddToInventory(string itemName)
     {
         int itemPosition = itemList.IndexOf(itemName);
         if (itemPosition != -1)
         {
            if (itemCount[itemPosition] <= maxQuantity)
            {
                itemCount[itemPosition]++;
                itemToAdd = Instantiate(Resources.Load<GameObject>(itemName));
                itemToAdd.transform.SetParent(slotList[itemPosition].transform);
                itemToAdd.transform.localPosition = Vector3.zero;
                itemToAdd.transform.localRotation = Quaternion.identity;

                TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);
                Debug.Log("Added 1 " + itemName + ". Current stack size: " + itemCount[itemPosition]);
            }
            else
            {
                Debug.Log("Inventory slot for " + itemName + " is full.");
            }

            /*itemCount[itemPosition]++;
            itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), slotList[itemPosition].transform.position, slotList[itemPosition].transform.rotation);

            TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

            Debug.Log("Added 1 " + itemName + ". Current stack size: " + itemCount[itemPosition]);*/
         }
        else
        {
            GameObject emptySlot = FindNextEmptySlot();
            if (emptySlot != null)
            {
                GameObject newItem = Instantiate(Resources.Load<GameObject>(itemName));
                newItem.transform.SetParent(emptySlot.transform);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localRotation = Quaternion.identity;
                itemList.Add(itemName);
                itemCount.Add(1);

                TriggerPickupPopUp(itemName, newItem.GetComponent<Image>().sprite);
                Debug.Log("Added 1 " + itemName + " to a new slot.");
            }
            else
            {
                Debug.Log("Inventory is full. Cannot add " + itemName + ".");
            }

            /*GameObject emptySlot = FindNextEmptySlot();
            if (emptySlot != null)
            {
                GameObject newItem = Instantiate(Resources.Load<GameObject>(itemName), emptySlot.transform.position, emptySlot.transform.rotation);
                newItem.transform.SetParent(emptySlot.transform);
                itemList.Add(itemName);
                itemCount.Add(1);


                //itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), slotList[itemPosition].transform.position, slotList[itemPosition].transform.rotation);
                //repara bug aici, nu afiseaza daca ridici primul produs
                //TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);
                Debug.Log("Added 1 " + itemName + " to a new slot.");
            }
            else
            {
                Debug.Log("Inventory is full. Cannot add " + itemName + ".");
            }*/
        }
    }

    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickUpAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
    }

    public void RecalculateList()
    {
        Debug.Log("reluare");
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove; //la noi e mai simplu ca stim deja cantitatile si unde se afla ele in inventar (sunt in acelasi slot)

        for(var i = slotList.Count - 1; i >= 0; i--) 
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "Clone" && counter > 0)
                {
                    counter = itemCount[i] - amountToRemove;
                    itemCount[i] -= amountToRemove; //reset the counter for the object
                }
                if(counter == 0)
                {
                    Destroy(slotList[i].transform.GetChild(0));
                    Debug.Log("All the quantity was used");
                    itemCount[i] = 0;
                }
                else if(counter < 0)
                {
                    Debug.Log("Not enaugh items");
                }
            }
        }
    }

    private int getItemPosition(string itemName)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == itemName)
                return i;
        }
        return -1;
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
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

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter++;
            }
        }
        if (counter > 24 * 10 + 1)
            return true;
        return false;
    }

    public void ConsumeItem(string itemName)
    {
        int itemPosition = itemList.IndexOf(itemName);
        if (itemPosition != -1)
        {
            if (itemCount[itemPosition] > 1)
            {
                itemCount[itemPosition]--; // Decrease the quantity
                Debug.Log("Consumed 1 " + itemName + ". Remaining quantity: " + itemCount[itemPosition]);
            }
            else
            {
                itemList.RemoveAt(itemPosition);
                itemCount.RemoveAt(itemPosition);
                Debug.Log("Consumed the last " + itemName + " from the inventory.");
            }
        }
        else
        {
            Debug.LogWarning("Item " + itemName + " not found in the inventory.");
        }
    }
}
