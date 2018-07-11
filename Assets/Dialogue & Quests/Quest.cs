using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// common data and behaviour for all quests
public class Quest : MonoBehaviour
{
    [SerializeField] string questName;

    public string GetQuestAsString()
    {
        return questName;
    }
	
}
