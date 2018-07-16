using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using System;

namespace RPG.Quests
{
    [RequireComponent(typeof(HealthSystem))]
    public class QuestKill : MonoBehaviour
    {
        [SerializeField] Quest questToComplete;

        // configuration parameters, consider SO

        // private instance variables for state

        // cached references for readability

        // messages, then public methods, then private methods...
        void Update()
        {
            if (IsHostDead())
            {
                FindObjectOfType<QuestJournal>().CompleteQuest(questToComplete);
            }
        }

        private bool IsHostDead()
        {
            var healhOfHost = GetComponent<HealthSystem>().healthAsPercentage;
            return healhOfHost <= Mathf.Epsilon;
        }
    }
}