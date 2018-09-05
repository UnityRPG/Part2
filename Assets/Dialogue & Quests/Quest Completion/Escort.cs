using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Questing
{
    [RequireComponent(typeof(Character))]
    public class Escort : QuestCompletion
    {
        // configuration parameters, consider SO

        // private instance variables for state
        Coroutine escortingRoutine = null;

        void Update()
        {
            var questStarted = questToComplete.QuestState == QuestState.Started;
            if (questStarted && escortingRoutine == null)
            {
                var player = GameObject.FindWithTag("Player");
                escortingRoutine = StartCoroutine(FollowPlayer(player));
            }
            if (!questStarted && escortingRoutine != null)
            {
                StopCoroutine(escortingRoutine);
            }
        }

        private IEnumerator FollowPlayer(GameObject player)
        {
            while (true)
            {
                GetComponent<Character>().SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        // Detect when at destination
        // Do something
    }
}