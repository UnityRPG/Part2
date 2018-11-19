using System.Collections;
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
        Coroutine behaviourRoutine;

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
            if (inWeaponCircle)
            {
                StartAttack();
            }
            else if (inChaseRing)
            {
                StartChase();
            }
            else if (hasPatrol)
            {
                StartPatrol();
            } else
            {
                StartIdle();
            }
        }

        bool inWeaponCircle => distanceToPlayer <= currentWeaponRange;
        bool inChaseRing => distanceToPlayer <= chaseRadius;
        bool hasPatrol => patrolPath != null;

        float distanceToPlayer => Vector3.Distance(player.transform.position, transform.position);

        float currentWeaponRange
        {
            get
            {
                if (weaponSystem == null) return 0;
                return weaponSystem.GetMaxAttackRange();
            }
        }

        private void StartAttack()
        {
            if (Transition(State.attacking))
            {
                weaponSystem.AttackTarget(player.gameObject);
            }
        }

        private void StartChase()
        {
            if (Transition(State.chasing))
            {
                behaviourRoutine = StartCoroutine(ChasePlayer());
            }
        }

        private void StartPatrol()
        {
            if (Transition(State.patrolling))
            {
                behaviourRoutine = StartCoroutine(Patrol());
            }
        }

        private void StartIdle()
        {
            Transition(State.idle);
        }

        private bool Transition(State newState)
        {
            if (state != newState)
            {
                state = newState;
                StopRunningBehaviours();
                return true;
            }

            return false;
        }

        private void StopRunningBehaviours()
        {
            weaponSystem.StopAttacking();
            character.ClearDestination();
            if (behaviourRoutine != null)
            {
                StopCoroutine(behaviourRoutine);
                behaviourRoutine = null;
            }
        }

        IEnumerator Patrol()
        {
            while (true)
            {
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose();
                yield return new WaitForSeconds(waypointDwellTime);
            }
        }

        IEnumerator ChasePlayer()
        {
            while (true)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }
        Vector3 nextWaypointPos => patrolPath.transform.GetChild(nextWaypointIndex).position;

        private void CycleWaypointWhenClose()
        {
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
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