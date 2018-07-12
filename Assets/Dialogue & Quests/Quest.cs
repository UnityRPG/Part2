using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// common data and behaviour for all quests
// TODO consider persistence across scene loads
public class Quest : MonoBehaviour
{
    enum QuestState { 
        Locked,  // some pre-requisite not met
        Unlocked,  // avilable to be issued
        Active,
        Complete
    }

    [SerializeField] string questName;  // TODO use gameobject name?
    [SerializeField] QuestState questState;

    public string GetQuestAsString()
    {
        return questName;
    }
	
}
