using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Quest completion criteria, idea is to compose more complex criteria
// Example other criteria: Kill, Delivery, Gather, Escort. Guess/Solve, Combo.

namespace RPG.Quests
{
    public class QuestCollision : MonoBehaviour // Not from quest
    {
        [SerializeField] Quest questToComplete;

        private void OnTriggerEnter(Collider other)
        {
            FindObjectOfType<QuestJournal>().CompleteQuest(questToComplete);
        }


    }
}