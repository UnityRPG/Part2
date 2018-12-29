using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using System;

namespace RPG.Questing.Completion
{
    [RequireComponent(typeof(HealthSystem))]
    public class Kill : QuestCompletion
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