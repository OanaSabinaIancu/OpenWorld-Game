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
    private Text itemInfoUI_itemDescription;
    private Image itemInfoUI_itemSprite; // Change to Image type

    public string thisName, thisDescription;
    public Sprite thisSprite; // Resource name for the sprite

    // --- Consumption --- //
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float staminaEffect;


    private void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<Text>();
        itemInfoUI_itemSprite = itemInfoUI.transform.Find("itemPicture").GetComponent<Image>();
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
    public void OnPointerDown(PointerEventData eventData)
    {
        // Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Find the position of the item name in the itemList
            int itemPosition = InventorySystem.Instance.itemList.IndexOf(thisName);

            // Check if itemCount at the itemPosition is higher than 0
            if (isConsumable && InventorySystem.Instance.itemCount[itemPosition] > 0)
            {
                // Setting this specific gameobject to be the item we want to destroy later
                itemPendingConsumption = gameObject;

                // Decrease the item count
                InventorySystem.Instance.itemCount[itemPosition]--;

                // Check if there are no more items left
                if (InventorySystem.Instance.itemCount[itemPosition] == 0)
                {
                    itemInfoUI_itemDescription.text = "Item not available";
                }

                consumingFunction(healthEffect, staminaEffect);
            }
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
