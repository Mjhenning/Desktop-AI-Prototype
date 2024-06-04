using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Set_Instance : MonoBehaviour {
    public ItemSprite_Instance itemDisplay;

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
                items[i].GetComponent<TextMeshProUGUI> ().text = "- " + set.items[i].itemName;
                    items[i].GetComponentInChildren<Image> ().sprite = set.items[i].itemSprite;
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

    public void ChangeDisplay (GameObject referenceObj) {
        itemDisplay.gameObject.SetActive (true);
        if (items.IndexOf(referenceObj) == 3) {
            itemDisplay.spriteObj.sprite = set.set.itemSprite;
            itemDisplay.textObj.text = "Name: " + set.set.itemName + "\n" + "Set & Type: " + set.set.mainSet + " Main Item" + "\n" + "Description: " + set.set.itemDesc;
        } else {
            itemDisplay.spriteObj.sprite = set.items[items.IndexOf (referenceObj)].itemSprite;
            itemDisplay.textObj.text = "Name: " + set.items[items.IndexOf (referenceObj)].itemName + "\n" + "Set & Type: " + set.items[items.IndexOf (referenceObj)].mainSet + " " + set.items[items.IndexOf (referenceObj)].itemType + "\n" + "Description: " + set.items[items.IndexOf (referenceObj)].itemDesc;
        }
    }

}
