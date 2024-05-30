using UnityEngine;

public class InventoryManager : MonoBehaviour {
    ItemDatabase inventory;
    
    void Start () {
        EventsManager.CheckDatabase.AddListener (EvaluateBool);

        inventory = SaveLoadManager.instance.inventory;
    }
    
    void EvaluateBool (ItemInstance _instance, bool buy) {
        if (buy) {
            AddToDatabase (_instance);
        } else {
            RemoveFromShop (_instance, ShopRemoveType.Discard);
        }
    }

    void AddToDatabase(ItemInstance instance) { //adds item to database scriptable and removes it from lists and removes cost from player goop count
         for (int i = 0; i < inventory.setDB.setList.Length; i++) {
             if (inventory.setDB.setList[i].set.mainSet == instance.assignedItem.mainSet) {
                 inventory.setDB.setList[i].items.Add(instance.assignedItem);
                 EventsManager.RemoveGoop(instance.assignedItem.itemGoopCost);
                 RemoveFromShop (instance, ShopRemoveType.Buy);
             }
         }
    }

    void RemoveFromShop (ItemInstance instance, ShopRemoveType type) {
        EventsManager.RemoveFromLists (instance, type);
    }
    

}
