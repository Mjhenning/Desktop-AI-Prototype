using System.Collections.Generic;
using UnityEngine;

//class used as a blueprint for savedata in save/load manager

[System.Serializable]
    public class Savedata {
        public List<ItemData> items;
        public VendorData vendor;
        public Playerdata player;
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

[System.Serializable]
public class VendorData {

    public Color currentTieColor = Color.white;
    public int currentVendorMaskIndex = 0;

}

[System.Serializable]
public class Playerdata {

    public double currentCPercentage;
    public double currentCorruption;

}
