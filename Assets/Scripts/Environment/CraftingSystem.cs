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
        if (Input.GetKeyDown(KeyCode.F) && !isOpen && SelectionManager.Instance.onTarget)
        {

            Debug.Log("f is pressed");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            //adauga blocarea miscarii caracterului
            //controller.Move(move * speed * Time.deltaTime);

            isOpen = true;


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
