using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Set_Instance : MonoBehaviour {

    public TextMeshProUGUI setName;
    public List<GameObject> items;
    public SetItemCombo set;

    public void PopulateDisplays () {
        foreach (GameObject item in items) {
            item.SetActive (true);
        }

        setName.text = set.set.mainSet.ToString ();

        if (set.items.Count == 3) {
            for (int i = 0; i < items.Count; i++) {
                items[i +1].GetComponent<TextMeshProUGUI> ().text = "- " + set.items[i].itemName;
                    items[i +1].GetComponentInChildren<Image> ().sprite = set.items[i].itemSprite;
            }

            items[3].GetComponent<TextMeshProUGUI> ().text = "- " + set.set.itemName;
            items[3].GetComponentInChildren<Image> ().sprite = set.set.itemSprite;
            
        } else if (set.items.Count < 3){
            for (int i = 0; i < items.Count; i++) {
                if (i < set.items.Count && set.items[i] != null) {
                    items[i].GetComponent<TextMeshProUGUI>().text = "- " + set.items[i].itemName;
                    items[i].GetComponentInChildren<Image>().sprite = set.items[i].itemSprite; 
                } else {
                    items[i].SetActive (false);
                }
                
            }
        }
        
       
    }
    
    
}
