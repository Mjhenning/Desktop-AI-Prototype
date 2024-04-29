using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemInstance : MonoBehaviour { //each item in ui uses this script to store it's item before it gets saved to database if bought

    public Item assignedItem;
    
    public Image image;
    [FormerlySerializedAs ("Text")] public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public void BuyItem () {
        EventsManager.BoughtItem (assignedItem);
    }
}
