using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// common data and behaviour for all quests
// TODO consider persistence across scene loads

public enum QuestState
{
    Available,
    Started,
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
