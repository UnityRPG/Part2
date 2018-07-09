using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("RPG/Conversation"))]
public class Conversation : ScriptableObject
{
    [TextArea]
    [SerializeField] string openingGambit;

    // TODO consdier creating cast from convo to string
    public string getConvoAsString()
    {
        return openingGambit;
    }
}
