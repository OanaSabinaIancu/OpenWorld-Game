using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    public GameObject craftingScreenUI;
    public bool isOpen;

    //declarations for object list
    public List<GameObject> slotList = new List<GameObject>(); //list of slots
    public List<string> itemList = new List<string>(); //list of object names
    public List<int> itemCount = new List<int>(); //list of quantity
    public int maxQuantity = 11;

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    //collider for th crafting bench
    private Collider other;

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
        foreach (Transform child in craftingScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && !isOpen && SpecialInteraction.Instance.OnTriggerEnter(other))
        {

            Debug.Log("f is pressed");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            //adauga blocarea miscarii caracterului
            //controller.Move(move * speed * Time.deltaTime);

            isOpen = true;


        }
        else if (Input.GetKeyDown(KeyCode.F) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }
}
