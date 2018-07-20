using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class Gather : QuestCompletion
    {
        void OnCollisionEnter() // 1st copy from Collision.cs
        {
            if (questToComplete.QuestState == QuestState.Started)
            {   
                Destroy(gameObject);
                // TODO put object in inventory
                CompleteQuest();
            }
        }
    }
}