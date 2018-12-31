using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Questing.Completion
{
    [RequireComponent(typeof(Mover))]
    public class Escort : QuestCompletion
    {
        // configuration parameters, consider SO

        // private instance variables for state
        Coroutine escortingRoutine = null;

        void Update()
        {
            bool questStarted = IsActive();
            if (IsActive() && escortingRoutine == null)
            {
                var player = GameObject.FindWithTag("Player");
                escortingRoutine = StartCoroutine(FollowPlayer(player));
            }
            if (!IsActive() && escortingRoutine != null)
            {
                StopCoroutine(escortingRoutine);
            }
        }

        private IEnumerator FollowPlayer(GameObject player)
        {
            while (true)
            {
                GetComponent<Mover>().SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        // Detect when at destination
        // Do something
    }
}