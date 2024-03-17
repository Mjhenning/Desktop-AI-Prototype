using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MAIN NOTE ARTIFACTS AND MAINSET ITEMS ARE NOT RANDOMIZED

public static class ItemNameGenerator {
    public static readonly List<string> NegativePrefixes = new List<string> {
        "Madness",
        "Ethereal",
        "Echoing",
        "Whispering",
        "Eerie",
        "Murmuring",
        "Forbidden",
        "Haunting",
        "Nightmare Inducing"
    };
    
    public static readonly List<string> NeutralPrefixes = new List<string> {
        "Ancient",
        "Cursed",
        "Enchanted",
        "Spectral",
        "Otherworldly",
        "Surreal",
        "Dream",
        "Reflection",
        "Mindreading",
        "Timekeeping",
        "Memory",
        "Shadow"
    };


    static readonly List<string> Relic = new List<string> {
        "Tablet",
        "Scroll",
        "Inscription",
        "Plaque",
        "Panel"
    };

    static readonly List<string> Ephemera = new List<string> {
        "Mirage",
        "Shade",
        "Whisper",
        "Ember",
        "Effigy"
    };

    // Method to generate an item name
    public static string GenerateItemName (SetTypes set,ItemTypes item) { //generates item name based off of fed set and item type

        switch (set) {
            case SetTypes.Abstracted: //Abstracted set
                return GenerateNeutralName (item);
            case SetTypes.Beating: //Beating set
                return GenerateNeutralName (item);
            case SetTypes.Corrupted: //Corrupted set
                return GenerateNegativeName (item);
            case SetTypes.Liminal: //Liminal set
                return GenerateNeutralName (item);
            case SetTypes.Submerged: //submerged set
                switch (item) {
                    case ItemTypes.Ephemera:
                        return GenerateNegativeName (item);
                    case ItemTypes.Relic:
                        return GenerateNeutralName (item);
                }
                break;
            case SetTypes.Whispering: //whispering set
                switch (item) {
                    case ItemTypes.Ephemera:
                        return GenerateNeutralName (item);
                    case ItemTypes.Relic:
                        return GenerateNegativeName (item);
                }
                break;
            case SetTypes.Speaking: //speaking set
                switch (item) {
                    case ItemTypes.Ephemera:
                        return GenerateNegativeName (item);
                    case ItemTypes.Relic:
                        return GenerateNeutralName (item);
                }
                break;
        }

        return "";
    }



    static string GenerateNeutralName (ItemTypes item) { //generates a neutral sounding name based off of neutral suffixes and relic or ephemera roots
        switch (item) {
            case ItemTypes.Ephemera:
                return NeutralPrefixes[Random.Range (0, NeutralPrefixes.Count)] + " " + Ephemera[Random.Range (0, Ephemera.Count)];
            case ItemTypes.Relic:
                return NeutralPrefixes[Random.Range (0, NeutralPrefixes.Count)] + " " + Relic[Random.Range (0, Relic.Count)];
            default:
                return "";
        }
    }

    static string GenerateNegativeName (ItemTypes item) { //generates a negative sounding name based off of negative suffixes and relic or ephemera roots
        switch (item) {
            case ItemTypes.Ephemera:
                return NegativePrefixes[Random.Range (0, NegativePrefixes.Count)] + " " + Ephemera[Random.Range (0, Ephemera.Count)];
            case ItemTypes.Relic:
                return NegativePrefixes[Random.Range (0, NegativePrefixes.Count)] + " " + Relic[Random.Range (0, Relic.Count)];
            default:
                return "";
        }
    }
}
