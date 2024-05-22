using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
    public class Savedata {
        public List<ItemData> items;
    }

[System.Serializable]
public class ItemData {

    public SetTypes mainSet;
    public ItemTypes itemType;
    
    public Sprite itemSprite;
    public string itemName;
    public string itemDesc;
        
    public int itemGoopCost;
        
}
