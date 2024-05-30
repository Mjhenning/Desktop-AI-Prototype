using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayParent : MonoBehaviour
{

    public List<ItemSprite_Instance> shopItemDisplays;
    
    void OnEnable () { //handles logic for populating sprite displays
        
        ShopInstance parent = GetComponentInParent<ShopInstance> ();
        
        for (int i = 0; i <  parent.shopItems.Count; i++) {
            shopItemDisplays[i].textObj.text = parent.shopItems[i].itemGoopCost.ToString ();
            if (parent.shopItems[i].itemSprite != null) {
                shopItemDisplays[i].spriteObj.sprite = parent.shopItems[i].itemSprite;
            }
            if (shopItemDisplays[i].gameObject.activeSelf == false) {
                shopItemDisplays[i].gameObject.SetActive (true);
            }
        }
    }

    void OnDisable () { //clears these sprite displays
        for (int i = 0; i < shopItemDisplays.Count; i++) { //activates objects before trying to access them
            if (shopItemDisplays[i].gameObject.activeSelf == true) {
                shopItemDisplays[i].gameObject.SetActive (false);
            }
            
        }
        
        for (int i = 0; i < shopItemDisplays.Count; i++) {
            
            shopItemDisplays[i].textObj.text = " ";
        }
    }
}
