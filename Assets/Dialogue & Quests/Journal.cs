using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Quests
{
    public class Journal : MonoBehaviour
    {
        // configuration parameters, consider SO

        // private instance variables for state

        // cached references for readability
        [SerializeField] Transform questParent;

        // messages, then public methods, then private methods...
        private void Update()
        {
            UpdateQuestsFromScene();
        }

        public void AddQuest(Quest quest)
        {
            GetComponent<Text>().text += quest.GetQuestAsString();
            quest.SetQuestState(QuestState.Started);
        }

        public void CompleteQuest(Quest quest)
        {
            GetComponent<Text>().text = "";
            quest.SetQuestState(QuestState.Complete);
        }

        private void UpdateQuestsFromScene()
        {
            GetComponent<Text>().text = "";
            foreach (Transform child in questParent)
            {
                ListQuestIfStarted(child);
            }
        }

        private void ListQuestIfStarted(Transform child)
        {
            var questState = child.gameObject.GetComponent<Quest>().GetQuestState();
            if (questState == QuestState.Started)
            {
                GetComponent<Text>().text += child.gameObject.name + '\n';
            }
        }
    }
}