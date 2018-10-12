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
        #region Configuration
        [SerializeField] protected string questIdToComplete;
        #endregion

        #region State
        Quest questToComplete;
        #endregion

        #region References
        Journal journal;
        #endregion

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