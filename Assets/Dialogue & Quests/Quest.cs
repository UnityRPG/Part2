using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Questing
{
    [Serializable]
    public class Quest
    {
        public string uniqueName;
        public string displayName;
        public int rewardCoin;

        // This code written by refactor tool
        public int RewardCoin
        {
            get
            {
                return rewardCoin;
            }
        }

        public string GetQuestAsString()
        {
            return displayName;
        }

        public string uniqueId
        {
            get
            {
                return uniqueName;
            }
        }
    }
}