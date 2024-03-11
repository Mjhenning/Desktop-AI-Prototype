using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes {
    Artifact,
    Relic,
    Ephemera,
    MainItem

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemSO", order = 1)]

public class Item : ScriptableObject { //scriptable setup for every single item in game (specify set item is part off, type of item, it's sprite, name, description and if it ahs been obtained)
    public SetTypes MainSet;
    public ItemTypes ItemType;
    
    public Sprite ITEM_Sprite;
    public string ItemName;
    public string ItemDesc;
    public bool Obtained;

}
