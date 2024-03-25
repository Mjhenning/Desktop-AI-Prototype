using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DatabaseSO", order = 1)]

public class ItemDatabase : ScriptableObject {
    [FormerlySerializedAs ("Sets")] public NewDict sets; //Dictionary containing sets
}

[Serializable]
public class NewDict : Dictionary<ItemSet, List<Item>> { //class containing array of sets
    [FormerlySerializedAs ("ListOfSets")] public NewDictItem[] listOfSets;
}

[Serializable]
public class NewDictItem { //class containing sets and items under set
    public ItemSet set;
    public List<Item> items;
}

