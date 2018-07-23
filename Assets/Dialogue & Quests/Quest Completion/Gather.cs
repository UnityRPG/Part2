using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    public class Gather : QuestCompletion
    {
        void OnCollisionEnter(Collision collision) // 1st copy from Collision.cs
        {
            if (questToComplete.QuestState == QuestState.Started)
            {   
                var player = GameObject.FindWithTag("Player");
                player.GetComponent<PlayerInventory>().AddToInventory(gameObject);
                gameObject.SetActive(false); // leave inactive "ghose" in scene
                CompleteQuest();
            }
        }
    }
}