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
            print(quest.name);
            activeQuests.Add(quest);
        }

        public void CompleteQuest(Quest quest)
        {
            activeQuests.Remove(quest);

            GetComponent<Text>().text = "";
        }

        public bool IsActiveQuest(Quest quest)
        {
            return activeQuests.Contains(quest);
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
            state["activeQuests"] = activeQuests[0].RewardCoin;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            throw new NotImplementedException();
        }
    }
}