using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Quest completion criteria, idea is to compose more complex criteria
// Example other criteria: Delivery, Gather, Escort. Guess/Solve, Combo

namespace RPG.Quests
{
    public class QuestCollision : QuestionCompletion // Not from quest
    {
        private void OnTriggerEnter(Collider other)
        {
            CompleteQuest();
        }
    }
}