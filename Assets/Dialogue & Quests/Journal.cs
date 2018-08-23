using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Questing
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
            quest.QuestState = QuestState.Started;
        }

        public void CompleteQuest(Quest quest)
        {
            GetComponent<Text>().text = "";
            quest.QuestState= QuestState.Complete;
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
            var questState = child.gameObject.GetComponent<Quest>().QuestState;
            if (questState == QuestState.Started)
            {
                GetComponent<Text>().text += child.gameObject.name + '\n';
            }
        }
    }
}