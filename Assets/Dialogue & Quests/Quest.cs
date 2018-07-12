using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// common data and behaviour for all quests
// TODO consider persistence across scene loads

[DisallowMultipleComponent]
public class Quest : MonoBehaviour
{
    enum QuestState { 
        Locked,  // some pre-requisite not met
        Unlocked,  // avilable to be issued
        Active,
        Complete
    }

    [SerializeField] QuestState questState;

    public string GetQuestAsString()
    {
        return gameObject.name;
    }
	
}
