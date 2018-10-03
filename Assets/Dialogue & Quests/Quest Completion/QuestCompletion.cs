using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Questing
{
    // Quest completion criteria, idea is to compose more complex criteria
    // Delivery (after inventory)

    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] protected string questIdToComplete;
        Quest questToComplete;

        // cached references for readability
        Journal journal;

        private void Start()
        {
            journal = FindObjectOfType<Journal>();
            questToComplete = journal.GetQuestById(questIdToComplete);
        }

        protected bool IsActive()
        {
            return journal.IsActiveQuest(questToComplete);
        }

        protected void CompleteQuest()
        {
            journal.CompleteQuest(questToComplete);
            EarnReward();
        }

        private void EarnReward()
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerInventory>().AddCoin(questToComplete.RewardCoin);
        }
    }
}