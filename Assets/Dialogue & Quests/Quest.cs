using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// common data and behaviour for all quests

namespace RPG.Characters
{
    public enum QuestState
    {
        Available,
        Started,
        Complete
    }

    [DisallowMultipleComponent]
    public class Quest : MonoBehaviour
    {
        [SerializeField] QuestState questState;
        [SerializeField] int rewardCoin;

        // This code written by refactor tool
        public int RewardCoin
        {
            get
            {
                return rewardCoin;
            }
        }

        public QuestState QuestState
        {
            get
            {
                return questState;
            }

            set
            {
                questState = value;
            }
        }

        public string GetQuestAsString()
        {
            return gameObject.name;
        }
    }
}