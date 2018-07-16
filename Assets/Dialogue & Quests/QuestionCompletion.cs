using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestionCompletion : MonoBehaviour
    {

        [SerializeField] protected Quest questToComplete;

        protected void CompleteQuest()
        {
            FindObjectOfType<QuestJournal>().CompleteQuest(questToComplete);
        }

    }
}