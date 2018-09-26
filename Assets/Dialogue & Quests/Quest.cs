using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    [CreateAssetMenu(menuName = ("RPG/Quest"))]
    public class Quest : ScriptableObject
    {
        [SerializeField] string questName;
        [SerializeField] int rewardCoin;

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
            return questName;
        }
    }
}