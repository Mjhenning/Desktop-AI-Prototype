using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList_Manager : MonoBehaviour {
    [SerializeField] ItemDatabase database;
    [SerializeField] List<Set_Instance> sets;

    void OnEnable () {
        for (int i = 0; i < sets.Count; i++) {
            sets[i].set = database.setDB.setList[i];
            sets[i].PopulateDisplays ();
        }
    }
}
