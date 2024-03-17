using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot : MonoBehaviour
{
    public GameObject trashAlertUI;

    private Text textToModify;

    public Sprite trash_closed;
    public Sprite trash_opened;

    private Image imageComponent;

    Button YesBTN, NoBTN;

    GameObject itemToBeDeleted;
    bool isItemDeleted = false; // Flag to track if item has been deleted

    public string itemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }

    void Start()
    {
        //imageComponent = transform.Find("background").GetComponent<Image>();

        //textToModify = trashAlertUI.transform.Find("Text").GetComponent<Text>();

        YesBTN = trashAlertUI.transform.Find("yes").GetComponent<Button>();
        YesBTN.onClick.AddListener(DeleteItem);

        NoBTN = trashAlertUI.transform.Find("no").GetComponent<Button>();
        NoBTN.onClick.AddListener(CancelDeletion);
    }

    public void SelectItemForDeletion(GameObject item)
    {
        if (item.GetComponent<InventoryItem>().isTrashable && !isItemDeleted) // Check if item is trashable and not already deleted
        {
            itemToBeDeleted = item;
            StartCoroutine(notifyBeforeDeletion());
        }
    }

    IEnumerator notifyBeforeDeletion()
    {
        trashAlertUI.SetActive(true);
        //textToModify.text = "Throw away this " + itemName + "?";
        yield return new WaitForSeconds(1f);
    }

    private void CancelDeletion()
    {
        //imageComponent.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }

    private void DeleteItem()
    {
        // Update item counter in InventorySystem
        int itemPosition = InventorySystem.Instance.itemList.IndexOf(itemToBeDeleted.GetComponent<InventoryItem>().thisName);
        if (itemPosition >= 0 && itemPosition < InventorySystem.Instance.itemCount.Count)
        {
            InventorySystem.Instance.itemCount[itemPosition] = 0;
        }

        // Set flag to true to indicate item has been deleted
        isItemDeleted = true;

        //imageComponent.sprite = trash_closed;
        //Destroy(itemToBeDeleted.gameObject);
        trashAlertUI.SetActive(false);
        Debug.Log("Item deleted: ");

    }

    // Add an event handler for left mouse button clicks
    public void OnPointerDown(PointerEventData eventData)
    {
        // Check if the left mouse button is clicked
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Object selected");

            // Get the item game object
            GameObject item = eventData.pointerCurrentRaycast.gameObject;

            InventorySystem.Instance.ItemInfoUI.SetActive(false);

            // Select the item for deletion
            SelectItemForDeletion(item);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            trashAlertUI.SetActive(false);
        }
    }
}
