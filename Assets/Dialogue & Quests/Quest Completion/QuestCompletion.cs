using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Quests
{
    // Quest completion criteria, idea is to compose more complex criteria
    // Gather (just before inventory)
    // Delivery (after inventory)

    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] protected Quest questToComplete;

        protected void CompleteQuest()
        {
            FindObjectOfType<Journal>().CompleteQuest(questToComplete);
            EarnReward();
        }

        private void EarnReward()
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerInventory>().AddCoin(questToComplete.RewardCoin);
        }
    }
}