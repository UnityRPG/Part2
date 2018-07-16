using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Quests
{
    [RequireComponent(typeof(Character))]
    public class Escort : QuestCompletion
    {
        // configuration parameters, consider SO

        // private instance variables for state
        bool isEscorting = false;

        // Follow the player IF IN RANGE
        // TODO couple with Collision quest critera

        void Update()
        {
            var questStarted = questToComplete.GetQuestState() == QuestState.Started;
            if (questStarted && isEscorting == false)
            {
                var player = GameObject.FindWithTag("Player");
                StartCoroutine(FollowPlayer(player));
                isEscorting = true;  // TODO really need a boolean flag?
            }
        }

        private IEnumerator FollowPlayer(GameObject player)
        {
            GetComponent<Character>().SetDestination(player.transform.position);
            yield return new WaitForEndOfFrame();
        }

        // Detect when at destination
        // Do something
    }
}