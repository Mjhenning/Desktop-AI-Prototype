using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DescriptionSO", order = 1)]
public class Descriptions: ScriptableObject { //scriptable used to store descriptions
    [FormerlySerializedAs ("Set")] public SetTypes set;
    [FormerlySerializedAs ("Verbs")] public List<string> verbs;
    [FormerlySerializedAs ("Adverbs")] public List<string> adverbs;
    [FormerlySerializedAs ("Adjectives")] public List<string> adjectives;
    [FormerlySerializedAs ("Feelings")] public List<string> feelings;
    [FormerlySerializedAs ("Eph_Extra")] public List<string> ephExtra;
    [FormerlySerializedAs ("Rel_Extra")] public List<string> relExtra;
}
