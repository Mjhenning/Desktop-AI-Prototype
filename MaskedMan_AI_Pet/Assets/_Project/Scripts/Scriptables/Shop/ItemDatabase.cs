using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DatabaseSO", order = 1)]

public class ItemDatabase : ScriptableObject {
    public NewDict Sets; //Dictionary containing sets
}

[Serializable]
public class NewDict : Dictionary<Item_Set, List<Item>> { //class containing array of sets
    public NewDictItem[] ListOfSets;
}

[Serializable]
public class NewDictItem { //class containing sets and items under set
    public Item_Set set;
    public List<Item> items;
}

