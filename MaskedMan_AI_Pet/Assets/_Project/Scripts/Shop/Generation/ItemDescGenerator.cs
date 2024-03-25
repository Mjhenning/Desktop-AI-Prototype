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
        
        string[] _words = item.itemName.Split (' '); //splits the item name into various strings saved in array words
        string _item;
        string _prefix;
        Descriptions _descriptionstring;

        if (_words.Length > 2) { //if words is more than 2 words
            _item = _words[2]; //use last word as root item
            _prefix = _words[0] + " " + _words[1]; //combine first 2 words into prefix
        } else { //else if word is less or equal
            _item = _words[1]; //set last word as root
            _prefix = _words[0]; //set first word as prefix
        }

        _descriptionstring = descriptions.Find (x => x.set == item.mainSet); //grab a description string so based off of the item's set

        switch (item.itemType) { //if item is an ephemera use template to generate descriptions
            case ItemTypes.Ephemera:
                return ("This " + _prefix + " " + _item +" is a " + item.itemType + " that "+ _descriptionstring.verbs[Random.Range(0, _descriptionstring.verbs.Count)] +" with " +
                        _descriptionstring.adverbs[Random.Range(0, _descriptionstring.adverbs.Count)] + " " + _descriptionstring.adjectives[Random.Range(0, _descriptionstring.adjectives.Count)] +" aura. It exudes a sense of " +
                        _descriptionstring.feelings[Random.Range(0, _descriptionstring.feelings.Count)] +" and " + _descriptionstring.feelings[Random.Range(0, _descriptionstring.feelings.Count)] + "." +
                        _descriptionstring.ephExtra[Random.Range(0, _descriptionstring.ephExtra.Count)]);
            case ItemTypes.Relic: //else if item is a relic use a different template to generate descriptions
                return ("This " + _prefix + " " + _item +" is an " + item.itemType + " that "+ _descriptionstring.verbs[Random.Range(0, _descriptionstring.verbs.Count)] +" with " +
                        _descriptionstring.adverbs[Random.Range(0, _descriptionstring.adverbs.Count)] + " " + _descriptionstring.adjectives[Random.Range(0, _descriptionstring.adjectives.Count)] +" aura. It evokes a sense of " +
                        _descriptionstring.feelings[Random.Range(0, _descriptionstring.feelings.Count)] +" and " + _descriptionstring.feelings[Random.Range(0, _descriptionstring.feelings.Count)] + "." +
                        _descriptionstring.relExtra[Random.Range(0, _descriptionstring.relExtra.Count)]);
        }
        
        return null;


    }

    
    
}
