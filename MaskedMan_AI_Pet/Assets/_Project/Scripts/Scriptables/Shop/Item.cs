using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum ItemTypes {
    Artifact,
    Relic,
    Ephemera,
    MainItem

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemSO", order = 1)]

public class Item : ScriptableObject { //scriptable setup for every single item in game (specify set item is part off, type of item, it's sprite, name, description and if it ahs been obtained)
    [FormerlySerializedAs ("MainSet")] public SetTypes mainSet;
    [FormerlySerializedAs ("ItemType")] public ItemTypes itemType;
    
    [FormerlySerializedAs ("ITEM_Sprite")] public Sprite itemSprite;
    [FormerlySerializedAs ("ItemName")] public string itemName;
    [FormerlySerializedAs ("ItemDesc")] public string itemDesc;

}
