using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    //Making class SelectionManager a singleton
    public static SelectionManager Instance { get; set; }

     public GameObject interaction_Info_UI;
     Text interaction_text;

    //Making the object targetable
    public bool onTarget;

    //pick up just the object we want
    public GameObject selectedObject;

    //As we can not create more than one instance for this class we use an awake method
    private void Awake()
    {
        if(Instance != null && Instance != this) 
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
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();


            if (interactable && interactable.playerInRange)
            {
                onTarget = true;

                selectedObject = interactable.gameObject;

                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else 
            { 
                onTarget = false;
                interaction_Info_UI.SetActive(false);
            }
 
        }
        else if (!Physics.Raycast(ray, out hit))
        {

            onTarget = false;
            interaction_Info_UI.SetActive(false);
        }
    }
}
