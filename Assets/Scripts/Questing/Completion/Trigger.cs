using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing.Completion
{
    public class Trigger : QuestCompletion // Not from quest
    {
        private void OnTriggerEnter(Collider other)
        {
            CompleteQuest();
        }
    }
}