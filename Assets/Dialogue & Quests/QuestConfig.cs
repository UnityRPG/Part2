using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Rick make some nice icons for these AssetMenu items
[CreateAssetMenu(menuName = "RPG/Quest")]
public class QuestConfig: ScriptableObject
{
    [SerializeField] string questName;

    public string GetQuestName()
    {
        return questName;
    }
}