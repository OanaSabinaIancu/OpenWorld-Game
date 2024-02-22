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

    //Category buttons
    Button toolsBTN;
    Button potionsBTN;
    Button craftingBTN;

    public bool isOpen;

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
        CraftItemButton.onClick.AddListener(delegate { CraftAnyItem(); });
    }

    private void CraftAnyItem()
    {
        throw new NotImplementedException();
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
            isOpen = false;
        }
    }
}
