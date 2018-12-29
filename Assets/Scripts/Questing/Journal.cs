using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using RPG.Core.Saving;

namespace RPG.Questing
{
    public class Journal : MonoBehaviour, ISaveable
    {
        // configuration parameters, consider SO
        [SerializeField] QuestList questList;

        // private instance variables for state
        List<Quest> activeQuests = new List<Quest>();

        // cached references for readability

        // messages, then public methods, then private methods...
        private void Update()
        {
            UpdateQuestsFromScene();
        }

        public void AddQuest(Quest quest)
        {
            activeQuests.Add(quest);
        }

        public void CompleteQuest(Quest quest)
        {
            activeQuests.Remove(quest);
        }

        public bool IsActiveQuest(Quest quest)
        {
            return activeQuests.Contains(quest);
        }

        public Quest GetQuestById(string questId)
        {
            return questList.GetQuestById(questId);
        }

        private void UpdateQuestsFromScene()
        {
            GetComponent<Text>().text = "";
            foreach (var quest in activeQuests)
            {
                GetComponent<Text>().text += quest.GetQuestAsString();
            }
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            var activeQuestIds = new string[activeQuests.Count];
            for (int i = 0; i < activeQuestIds.Length; i++)
            {
                activeQuestIds[i] = activeQuests[i].uniqueId;
            }
            state["activeQuests"] = activeQuestIds;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            activeQuests.Clear();
            var activeQuestIds = (string[])state["activeQuests"];
            for (int i = 0; i < activeQuestIds.Length; i++)
            {
                var quest = questList.GetQuestById(activeQuestIds[i]);
                activeQuests.Add(quest);
            }
        }
    }
}