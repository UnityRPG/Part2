using System.Collections;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;

namespace RPG.Control
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
        float timeLeftAtWaypoint;

        void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        void Update()
        {
            ClearBehaviours();

            if (inWeaponCircle)
            {
                PerformAttackBehaviour();
            }
            else if (inChaseRing)
            {
                PerformChaseBehaviour();
            }
            else if (hasPatrol)
            {
                PerformPatrolBehaviour();
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

        private void ClearBehaviours()
        {
            weaponSystem.StopAttacking();
            character.ClearDestination();
        }

        private void PerformAttackBehaviour()
        {
            weaponSystem.AttackTarget(player.gameObject);
        }

        private void PerformChaseBehaviour()
        {
            character.SetDestination(player.transform.position);
        }

        private void PerformPatrolBehaviour()
        {
            if (timeLeftAtWaypoint > 0)
            {
                timeLeftAtWaypoint -= Time.deltaTime;
            }
            else
            {
                character.SetDestination(nextWaypointPos);
                if (isAtWaypoint)
                {
                    CycleWaypoint();
                    timeLeftAtWaypoint = waypointDwellTime;
                }
            }
        }

        bool isAtWaypoint => Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance;
        Vector3 nextWaypointPos => patrolPath.transform.GetChild(nextWaypointIndex).position;

        private void CycleWaypoint()
        {
            nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
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