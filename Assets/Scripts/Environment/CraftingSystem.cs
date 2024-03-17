using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    //tab switching buttons
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;
    //public GameObject potionsScreenUI;

    //Craft Button
    Button CraftItemButton;

    //Requirement text
    Text ItemReq1, ItemReq2, ItemReq3;

    //All blueprints

    public List<string> inventoryItemList = new List<string>();
    public List<int> inventoryItemCount = new List<int>();

    //Category buttons
    Button toolsBTN;
    Button potionsBTN;
    Button craftingBTN;

    public Blueprint itemBlueprint = new Blueprint("Sword", 3, "Lily", 1, "Crafting Sword", 3, "Iron");

    public bool isOpen;

    //All blueprints


    //declarations for object list
    
    public List<int> itemCount = new List<int>(); //list of quantity
    
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

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        /*potionsBTN = craftingScreenUI.transform.Find("PotionssButton").GetComponent<Button>();
        potionsBTN.onClick.AddListener(delegate { OpenPotionsCategory();  });
        
        craftingBTN = craftingScreenUI.transform.Find("CraftingButton").GetComponent<Button>();
        potionsBTN.onClick.AddListener(delegate { OpenCraftingCategory();  });*/

        //Item
        ItemReq1 = toolsScreenUI.transform.Find("Item").transform.Find("req1").GetComponent<Text>();
        ItemReq2 = toolsScreenUI.transform.Find("Item").transform.Find("req2").GetComponent<Text>();
        ItemReq3 = toolsScreenUI.transform.Find("Item").transform.Find("req3").GetComponent<Text>();

        CraftItemButton = toolsScreenUI.transform.Find("Item").transform.Find("Button").GetComponent<Button>();
        CraftItemButton.onClick.AddListener(delegate { CraftAnyItem(itemBlueprint); });
    }

    private void CraftAnyItem(Blueprint itemBlueprint)
    {
        //add item into inventory
        InventorySystem.Instance.AddToInventory(itemBlueprint.name);

        //remove resources from the inventory
        InventorySystem.Instance.RemoveItem(itemBlueprint.req1, itemBlueprint.req1Amount);
        InventorySystem.Instance.RemoveItem(itemBlueprint.req2, itemBlueprint.req2Amount);
        InventorySystem.Instance.RemoveItem(itemBlueprint.req3, itemBlueprint.req3Amount);

        //recalculate the number of remaining items
        //InventorySystem.Instance.RecalculateList();

        //modifica maine cu chatgpt
        StartCoroutine(calculate());

        //refresh the needed items
        RefreshNeededItems();
    }

    private void RefreshNeededItems()
    {
        //doua variabile

        inventoryItemList = InventorySystem.Instance.itemList;
        inventoryItemCount = InventorySystem.Instance.itemCount;

        ItemReq1.text = inventoryItemCount[0].ToString() + "/3";
        ItemReq2.text = inventoryItemCount[1].ToString() + "/3";
        ItemReq3.text = inventoryItemCount[2].ToString() + "/3";

        if(inventoryItemCount[0] >= 3 && inventoryItemCount[1] >=3 && inventoryItemCount[2] >=3)
        {
            craftingBTN.gameObject.SetActive(true);
        }
        else
        {
            craftingBTN.gameObject.SetActive(false);
        }
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        //toolsScreenUI.SetActive(true);
    } 
    
    /*void OpenPotionsCategory()
    {
        craftingScreenUI.SetActive(false);
        potionsScreenUI.SetActive(true);
    }
    
    void OpenCraftingCategory()
    {
        craftingScreenUI.SetActive(true);
        potionsScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
    }*/

    void Update()
    {
        //modifica conditia sa deschida craft bench-ul doar daca e aproape de unul
        if (Input.GetKeyDown(KeyCode.F) && !isOpen && SelectionManager.Instance.onTarget)
        {
            // Check if the object being interacted with is not a crafting bench
            InteractableObject interactableObject = SelectionManager.Instance.selectedObject.GetComponent<InteractableObject>();
            if (interactableObject != null && interactableObject.GetItemName().Equals("Crafting-Bench"))
            {
                Debug.Log("f is pressed");
                craftingScreenUI.SetActive(true);
                //blocking character's movement
                PlayerMovement.Instance.controller.enabled = false;
                Cursor.lockState = CursorLockMode.None;

                //adauga blocarea miscarii caracterului
                //controller.Move(move * speed * Time.deltaTime);

                isOpen = true;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape)) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            //toolsScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            //blocking character's movement
            PlayerMovement.Instance.controller.enabled = true;
            isOpen = false;
        }
    }

    public IEnumerator calculate()
    {
        yield return 0;

        InventorySystem.Instance.RecalculateList();
    }
}
