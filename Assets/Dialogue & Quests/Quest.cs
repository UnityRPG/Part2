using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// common data and behaviour for all quests
// TODO consider persistence across scene loads

public enum QuestState
{
    Locked,  // some pre-requisite not met
    Unlocked,  // avilable to be issued
    Active,
    Complete
}

[DisallowMultipleComponent]
public class Quest : MonoBehaviour
{
    [SerializeField] QuestState questState;

    public QuestState GetQuestState()
    {
        return questState;
    }

    public void SetQuestState(QuestState questState)
    {
        this.questState = questState;
    }

    public string GetQuestAsString()
    {
        return gameObject.name;
    }
	
}
