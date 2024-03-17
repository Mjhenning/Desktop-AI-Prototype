using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DescriptionSO", order = 1)]
public class Descriptions: ScriptableObject { //scriptable used to store descriptions
    public SetTypes Set;
    public List<string> Verbs;
    public List<string> Adverbs;
    public List<string> Adjectives;
    public List<string> Feelings;
    public List<string> Eph_Extra;
    public List<string> Rel_Extra;
}
