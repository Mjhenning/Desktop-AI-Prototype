using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DatabaseSO", order = 1)]

public class ItemDatabase : ScriptableObject {
    [FormerlySerializedAs ("Sets")] public AllSets setDB; //Dictionary containing sets
}

[Serializable]
public class AllSets : Dictionary<ItemSet, List<Item>> { //class containing array of sets
    [FormerlySerializedAs ("ListOfSets")] public SetItemCombo[] setList;
}

[Serializable]
public class SetItemCombo{ //class containing sets and items under set
    public ItemSet set;
    public List<Item> items = new List<Item>();

    public void ClearItems () {
        items.Clear ();
    }
}


