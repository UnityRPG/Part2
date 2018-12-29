using UnityEngine;
using RPG.Combat;

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