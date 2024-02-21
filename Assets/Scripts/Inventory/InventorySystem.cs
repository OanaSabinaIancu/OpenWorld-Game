using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public bool isOpen;

    //declarations for object list
    public List<GameObject> slotList = new List<GameObject>(); //list of slots
    public List<string> itemList = new List<string>(); //list of object names
    public List<int> itemCount = new List<int>(); //list of quantity
    public int maxQuantity = 11;

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    //public bool isFull;

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
        //isFull = false;

        PopulateSlotList();

        //adauga 0 pentru fiecare item

        foreach (string item in itemList)
        {
            itemCount.Add(0);
        }
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

            //adauga blocarea miscarii caracterului
            //controller.Move(move * speed * Time.deltaTime);

            isOpen = true;


        }
        else if (Input.GetKeyDown(KeyCode.B) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName)
    {
        int itemPosition = itemList.IndexOf(itemName);
        if (itemPosition != -1)
        {
            if (itemCount[itemPosition] < maxQuantity)
            {
                itemCount[itemPosition]++;
                Debug.Log("Added 1 " + itemName + ". Current stack size: " + itemCount[itemPosition]);
            }
            else
            {
                Debug.Log("Inventory slot for " + itemName + " is full.");
            }
        }
        else
        {
            GameObject emptySlot = FindNextEmptySlot();
            if (emptySlot != null)
            {
                GameObject newItem = Instantiate(Resources.Load<GameObject>(itemName), emptySlot.transform.position, emptySlot.transform.rotation);
                newItem.transform.SetParent(emptySlot.transform);
                itemList.Add(itemName);
                itemCount.Add(1);
                Debug.Log("Added 1 " + itemName + " to a new slot.");
            }
            else
            {
                Debug.Log("Inventory is full. Cannot add " + itemName + ".");
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
}
