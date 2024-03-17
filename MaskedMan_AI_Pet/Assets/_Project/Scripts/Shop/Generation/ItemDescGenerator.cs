using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MAIN NOTE ARTIFACTS AND MAINSET ITEMS ARE NOT RANDOMIZED

public static class ItemDescGenerator {

    // Method to generate an item descriptions
    public static string GenerateItemDesc (Item item, List<Descriptions> descriptions) { //generates item description for fed item and list of descriptions scriptable objects (returns string)

        if (GenerateDescBasedOffName (item, descriptions) != null) { //if generatedescbasedoffname doesn't return nothing return it
            return GenerateDescBasedOffName (item, descriptions);
        } else {
            return "";
        }
    }

    public static string GenerateDescBasedOffName (Item item, List<Descriptions> descriptions) { //generates item description based off of the item fed and a list of descriptions scriptable objects
        
        string[] words = item.ItemName.Split (' '); //splits the item name into various strings saved in array words
        string _item;
        string _prefix;
        Descriptions descriptionstring;

        if (words.Length > 2) { //if words is more than 2 words
            _item = words[2]; //use last word as root item
            _prefix = words[0] + " " + words[1]; //combine first 2 words into prefix
        } else { //else if word is less or equal
            _item = words[1]; //set last word as root
            _prefix = words[0]; //set first word as prefix
        }

        descriptionstring = descriptions.Find (x => x.Set == item.MainSet); //grab a description string so based off of the item's set

        switch (item.ItemType) { //if item is an ephemera use template to generate descriptions
            case ItemTypes.Ephemera:
                return ("This " + _prefix + " " + _item +" is a " + item.ItemType + " that "+ descriptionstring.Verbs[Random.Range(0, descriptionstring.Verbs.Count)] +" with " +
                        descriptionstring.Adverbs[Random.Range(0, descriptionstring.Adverbs.Count)] + " " + descriptionstring.Adjectives[Random.Range(0, descriptionstring.Adjectives.Count)] +" aura. It exudes a sense of " +
                        descriptionstring.Feelings[Random.Range(0, descriptionstring.Feelings.Count)] +" and " + descriptionstring.Feelings[Random.Range(0, descriptionstring.Feelings.Count)] + "." +
                        descriptionstring.Eph_Extra[Random.Range(0, descriptionstring.Eph_Extra.Count)]);
            case ItemTypes.Relic: //else if item is a relic use a different template to generate descriptions
                return ("This " + _prefix + " " + _item +" is an " + item.ItemType + " that "+ descriptionstring.Verbs[Random.Range(0, descriptionstring.Verbs.Count)] +" with " +
                        descriptionstring.Adverbs[Random.Range(0, descriptionstring.Adverbs.Count)] + " " + descriptionstring.Adjectives[Random.Range(0, descriptionstring.Adjectives.Count)] +" aura. It evokes a sense of " +
                        descriptionstring.Feelings[Random.Range(0, descriptionstring.Feelings.Count)] +" and " + descriptionstring.Feelings[Random.Range(0, descriptionstring.Feelings.Count)] + "." +
                        descriptionstring.Rel_Extra[Random.Range(0, descriptionstring.Rel_Extra.Count)]);
        }
        
        return null;


    }

    
    
}
