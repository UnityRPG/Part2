using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using System;

namespace RPG.Quests
{
    [RequireComponent(typeof(HealthSystem))]
    public class QuestKill : QuestionCompletion
    {
        void Update()
        {
            if (IsHostDead())
            {
                CompleteQuest();
            }
        }

        private bool IsHostDead()
        {
            var healhOfHost = GetComponent<HealthSystem>().healthAsPercentage;
            return healhOfHost <= Mathf.Epsilon;
        }
    }
}