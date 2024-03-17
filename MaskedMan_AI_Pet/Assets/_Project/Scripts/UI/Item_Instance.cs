using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_Instance : MonoBehaviour { //each item in ui uses this script to store it's item before it gets saved to databse if bought

    public Item assignedItem;
    
    public Image image;
    public TextMeshProUGUI Text;
}
