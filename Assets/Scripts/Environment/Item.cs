using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item 
{
    public string itemName;
    public int quantity;
    public Sprite image;

    public Item(string itemName, int quantity, Sprite image)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.image = image;
    }
}
