using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SetTypes {
    Abstracted,
    Liminal,
    Corrupted,
    Whispering,
    Speaking,
    Beating,
    Submerged
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemSetSO", order = 1)]

public class Item_Set : Item { //scriptable setup for the main item of a set, only adds a list of the specific item types needed in set, a list of the items in the set (small database) and a list of bools determining which items have been obtained
    public List<ItemTypes> SetParts;
    public List<Item> SetItemDatabase;
}
