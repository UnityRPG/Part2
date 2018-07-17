using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Quests
{
    // Quest completion criteria, idea is to compose more complex criteria
    // Example other quest criteria TODO
    // Escort
    // Gather (just before inventory)
    // Delivery (after inventory)
    // Combo if required @Rick?
    // Guess / Solve if required @Rick?

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
            player.GetComponent<Inventory>().AddCoin(questToComplete.RewardCoin);
        }
    }
}