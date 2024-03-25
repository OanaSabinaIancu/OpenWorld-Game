using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    // --- Is this item trashable --- //
    public bool isTrashable;

    // --- Item Info UI --- //
    private GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    public Text itemInfoUI_itemDescription;
    private Image itemInfoUI_itemSprite; // Change to Image type

    public string thisName, thisDescription;
    public Sprite thisSprite; // Resource name for the sprite

    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float staminaEffect;

    //Equiping

    public bool isEquippable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuickSlot;

    private bool isConsumed;

    public bool isSelected;

    public static InventoryItem Instance { get; set; }


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
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<Text>();
        itemInfoUI_itemSprite = itemInfoUI.transform.Find("itemPicture").GetComponent<Image>();

        isConsumed = false;

        //selectedItems.SetActive(selectedItems.activeSelf);
    }

    private void Update()
    {
        // Check if the item is not null and if it has a valid position in the item list
        if (!string.IsNullOrEmpty(thisName))
        {
            int itemPosition = InventorySystem.Instance.itemList.IndexOf(thisName);

            // Ensure that the item position is valid
            if (itemPosition >= 0 && itemPosition < InventorySystem.Instance.itemCount.Count)
            {
                // Check if the item is consumed
                isConsumed = InventorySystem.Instance.itemCount[itemPosition] <= 0;

                // Handle consuming the item
                if (Input.GetKeyDown(KeyCode.Z) && !isConsumed)
                {
                    // Check if the item is consumable
                    if (isConsumable)
                    {
                        // Setting this specific gameobject to be the item we want to destroy later
                        itemPendingConsumption = gameObject;

                        // Display appropriate message
                        if (InventorySystem.Instance.itemCount[itemPosition] < 0)
                        {
                            itemInfoUI_itemDescription.text = "Item not available";
                        }
                        else 
                        {
                            itemInfoUI_itemDescription.text = "Item was consumed. You have " + InventorySystem.Instance.itemCount[itemPosition] + " items remaining";
                        }

                        
                        if (InventorySystem.Instance.itemCount[itemPosition] > 0)
                        {
                            itemInfoUI_itemDescription.text = "Item was consumed. You have " + InventorySystem.Instance.itemCount[itemPosition] + " items remaining";
                            consumingFunction(healthEffect, staminaEffect);
                            // Decrease the item count
                            InventorySystem.Instance.itemCount[itemPosition]--;
                        }
                        
                        else 
                        {
                            itemInfoUI_itemDescription.text = "Item was not consumed. You do not have any items remaining";
                        }
                    }
                }
            }
            else if(itemPosition < 0 || itemPosition > InventorySystem.Instance.itemCount.Count)
            {
                itemInfoUI_itemDescription.text = "Item not available";
            }
           
        }
    }



    // Triggered when the mouse enters into the area of the item that has this script.
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemSprite.sprite = thisSprite;
    }

    // Triggered when the mouse exits the area of the item that has this script.
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    // Triggered when the mouse is clicked over the item that has this script.
    // Triggered when the mouse is clicked over the item that has this script.
    public void OnPointerDown(PointerEventData eventData)
    {
        int itemPosition = InventorySystem.Instance.itemList.IndexOf(thisName);

        // Check if the itemPosition index is valid
        if (itemPosition >= 0 && eventData.button == PointerEventData.InputButton.Right || itemPosition >= 0)
        {
            // Right Mouse Button Click on
            if (eventData.button == PointerEventData.InputButton.Right && InventorySystem.Instance.itemCount[itemPosition] > 0)
            {
                // Check if the item is consumable
                if (isConsumable)
                {
                    // Setting this specific gameobject to be the item we want to destroy later
                    itemPendingConsumption = gameObject;

                    // Decrease the item count
                    InventorySystem.Instance.itemCount[itemPosition]--;

                    // Check if there are no more items left
                    if (InventorySystem.Instance.itemCount[itemPosition] < 0)
                    {
                        itemInfoUI_itemDescription.text = "Item not available";
                    }
                    else
                    {
                        itemInfoUI_itemDescription.text = "Item was consumed. You have " + InventorySystem.Instance.itemCount[itemPosition] + " items remaining";
                    }

                    consumingFunction(healthEffect, staminaEffect);
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            itemInfoUI_itemDescription.text = "Item can not be consumed";
        }
        else if (itemPosition >= 0 && eventData.button == PointerEventData.InputButton.Left)
        {
            // Check if the item is trashable
            if (isTrashable)
            {
                TrashSlot trashSlot = FindObjectOfType<TrashSlot>();
                trashSlot.SelectItemForDeletion(gameObject);
            }
            Debug.Log("Would you like to delete this object?");
        }
        else
        {
            isTrashable = false;
        }

        if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
        {
            EquipSystem.Instance.AddToQuickSlots(gameObject);
            isInsideQuickSlot = true;
        }
    }

    // Triggered when the mouse button is released over the item that has this script.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                InventorySystem.Instance.ConsumeItem(thisName); // Call InventorySystem method to consume the item
            }
        }
    }

    private void consumingFunction(float healthEffect, float staminaEffect)
    {
        int itemPosition = InventorySystem.Instance.itemList.IndexOf(thisName);
        if (InventorySystem.Instance.itemCount[itemPosition] == 1)
        {
            healthEffectCalculation(healthEffect);
            staminaEffectCalculation(staminaEffect);
            itemInfoUI.SetActive(false);
        }
    }

    private void healthEffectCalculation(float healthEffect)
    {
        // --- Health --- //
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            float newHealth = Mathf.Min(healthBeforeConsumption + healthEffect, maxHealth);
            PlayerState.Instance.setHealth(newHealth); // Assuming PlayerState has a SetHealth method
        }
    }

    private void staminaEffectCalculation(float staminaEffect)
    {
        // --- Stamina --- //
        float staminaBeforeConsumption = PlayerState.Instance.currentStamina;
        float maxStamina = PlayerState.Instance.maxStamina;

        if (staminaEffect != 0)
        {
            float newStamina = Mathf.Min(staminaBeforeConsumption + staminaEffect, maxStamina);
            PlayerState.Instance.setStamina(newStamina); // Assuming PlayerState has a SetStamina method
        }
    }
}
