using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MAIN NOTE ARTIFACTS AND MAINSET ITEMS ARE NOT RANDOMIZED

public static class ItemDescGenerator {

    // Method to generate an item descriptions
    public static string GenerateItemDesc (Item item, List<Descriptions> descriptions) {

        if (GenerateDescBasedOffName (item, descriptions) != null) {
            return GenerateDescBasedOffName (item, descriptions);
        } else {
            return "";
        }
    }

    public static string GenerateDescBasedOffName (Item item, List<Descriptions> descriptions) {
        
        string[] words = item.ItemName.Split (' ');
        string _item;
        string _prefix;
        Descriptions descriptionstring;

        if (words.Length > 2) {
            _item = words[2];
            _prefix = words[0] + " " + words[1];
        } else {
            _item = words[1];
            _prefix = words[0];
        }

        descriptionstring = descriptions.Find (x => x.Set == item.MainSet);

        switch (item.ItemType) {
            case ItemTypes.Ephemera:
                return ("This " + _prefix + " " + _item +" is a " + item.ItemType + " that "+ descriptionstring.Verbs[Random.Range(0, descriptionstring.Verbs.Count)] +" with " +
                        descriptionstring.Adverbs[Random.Range(0, descriptionstring.Adverbs.Count)] + " " + descriptionstring.Adjectives[Random.Range(0, descriptionstring.Adjectives.Count)] +" aura. It exudes a sense of " +
                        descriptionstring.Feelings[Random.Range(0, descriptionstring.Feelings.Count)] +" and " + descriptionstring.Feelings[Random.Range(0, descriptionstring.Feelings.Count)] + "." +
                        descriptionstring.Eph_Extra[Random.Range(0, descriptionstring.Eph_Extra.Count)]);
            case ItemTypes.Relic:
                return ("This " + _prefix + " " + _item +" is an " + item.ItemType + " that "+ descriptionstring.Verbs[Random.Range(0, descriptionstring.Verbs.Count)] +" with " +
                        descriptionstring.Adverbs[Random.Range(0, descriptionstring.Adverbs.Count)] + " " + descriptionstring.Adjectives[Random.Range(0, descriptionstring.Adjectives.Count)] +" aura. It evokes a sense of " +
                        descriptionstring.Feelings[Random.Range(0, descriptionstring.Feelings.Count)] +" and " + descriptionstring.Feelings[Random.Range(0, descriptionstring.Feelings.Count)] + "." +
                        descriptionstring.Rel_Extra[Random.Range(0, descriptionstring.Rel_Extra.Count)]);
        }
        
        return null;


    }

    
    
}
