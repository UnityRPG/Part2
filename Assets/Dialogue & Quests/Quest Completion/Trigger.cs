using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class Trigger : QuestCompletion // Not from quest
    {
        private void OnTriggerEnter(Collider other)
        {
            CompleteQuest();
        }
    }
}