using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestJournal : MonoBehaviour  // todo really needs to be?
{
    // configuration parameters, consider SO

    // private instance variables for state

    // cached references for readability

    // messages, then public methods, then private methods...
    public void AddQuest(Quest quest)
    {
        // TODO get actual quest description
        GetComponent<Text>().text += quest.GetQuestAsString();
        quest.SetQuestState(QuestState.Active);
    }

    public void CompleteQuest(Quest quest)
    {
        print("completing " + quest);
        GetComponent<Text>().text = "";
        quest.SetQuestState(QuestState.Complete);
    }
}
