using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]

    // TODO consider specialising to NPCMovement
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] float waypointDwellTime = 2.0f;

        PlayerControl player = null;
        Character character;
        WeaponSystem weaponSystem;

        int nextWaypointIndex;
        float currentWeaponRange;
        float distanceToPlayer;

        enum State { idle, patrolling, attacking, chasing }
        State state;

        void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            currentWeaponRange = weaponSystem.GetMaxAttackRange();

            bool inWeaponCircle = distanceToPlayer <= currentWeaponRange;
            bool inChaseRing = distanceToPlayer > currentWeaponRange 
                               && 
                               distanceToPlayer <= chaseRadius;
 
            if (inWeaponCircle)
            {
                StartAttack();
            }
            else if (inChaseRing)
            {
                StartChase();
            }
            else
            {
                StartPatrol();
            }
        }

        private void StartAttack()
        {
            if (state != State.attacking)
            {
                if (gameObject.tag == "Watch")
                {
                    Debug.Log("StartAttack", gameObject);
                }
                StopAllCoroutines();
                state = State.attacking;
                weaponSystem.AttackTarget(player.gameObject);
            }
        }

        private void StartChase()
        {
            if (state != State.chasing)
            {
                state = State.chasing;
                if (gameObject.tag == "Watch")
                {
                    Debug.Log("StartChase", gameObject);
                }
                weaponSystem.StopAttacking();
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
        }

        private void StartPatrol()
        {
            if (state != State.patrolling)
            {
                state = State.patrolling;
                if (gameObject.tag == "Watch")
                {
                    Debug.Log("StartPatrol", gameObject);
                }
                weaponSystem.StopAttacking();
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }
        }

        IEnumerator Patrol()
        {
            while (patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(waypointDwellTime);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }

        IEnumerator ChasePlayer()
        {
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        void OnDrawGizmos()
        {
            // Draw attack sphere 
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}