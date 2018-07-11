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
    public void AddQuest(QuestConfig questConfig)
    {
        GetComponent<Text>().text += questConfig.GetQuestName() + '\n';
    }

    public void CompleteQuest(QuestConfig questConfig)
    {
        GetComponent<Text>().text = "";
    }
}
