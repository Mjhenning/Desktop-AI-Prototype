using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_Instance : MonoBehaviour {

    Item assignedItem;
    
    [SerializeField]Image image;
    [SerializeField]TextMeshProUGUI Text;

    void Start() {

        assignedItem = ItemGenerator.instance.GrabGeneratedItem ();
        image.sprite = assignedItem.ITEM_Sprite;
        Text.text = "Item: " + assignedItem.ItemName + "\n" + "Type: " + assignedItem.ItemType + "\n" + assignedItem.ItemDesc;
    }
}
