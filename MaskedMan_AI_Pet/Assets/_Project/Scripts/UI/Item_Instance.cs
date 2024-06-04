using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemInstance : MonoBehaviour { //each item in ui uses this script to store it's item before it gets saved to database if bought

    public Item assignedItem;
    public Image image;
    [FormerlySerializedAs ("Text")] public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Button buyBtn;
    
    public void BuyItem () { //used to buy and item and purify corruption
        EventsManager.BoughtItem (this, true);
        Corruption_Manager.instance.Purify ();
    }

    public void DiscardItem () { //used to discard an item and corrupt vendor
        EventsManager.BoughtItem (this, false);
        Corruption_Manager.instance.Corrupt ();
    }

    void Update () { //checks if item can be bought otherwise button isn't interactable

        if (buyBtn != null) {
            buyBtn.interactable = CurrencyManager.instance.CheckCurrency(assignedItem.itemGoopCost);
        }
    }
    
}
