using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Start is called before the first frame update
    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }
}
