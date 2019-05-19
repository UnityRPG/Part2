using System.Collections;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.SpecialActions;

namespace RPG.Control
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(WeaponSystem))]

    // TODO consider specialising to NPCMovement
    public class EnemyAI : MonoBehaviour, IRaycastable
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] float waypointDwellTime = 2.0f;

        PlayerControl player = null;
        Mover mover;
        WeaponSystem weaponSystem;

        int nextWaypointIndex;
        float timeLeftAtWaypoint;

        void Start()
        {
            mover = GetComponent<Mover>();
            player = FindObjectOfType<PlayerControl>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        void Update()
        {
            ClearAll();
            if (inChaseRing)
            {
                if (weaponSystem)
                {
                    PerformAttackBehaviour();
                }
                else
                {
                    PerformFollowBehaviour();
                }
            }
            else if (hasPatrol)
            {
                PerformPatrolBehaviour();
            }
        }

        bool inChaseRing => distanceToPlayer <= chaseRadius;
        bool hasPatrol => patrolPath != null;

        float distanceToPlayer => Vector3.Distance(player.transform.position, transform.position);

        private void ClearAll()
        {
            weaponSystem.StopAttacking();
            mover.StopMovementAction();
        }

        private void PerformAttackBehaviour()
        {
            weaponSystem.AttackTarget(player.gameObject);
        }

        private void PerformFollowBehaviour()
        {
            mover.StartMovementAction(player.transform.position);
        }

        private void PerformPatrolBehaviour()
        {
            if (timeLeftAtWaypoint > 0)
            {
                timeLeftAtWaypoint -= Time.deltaTime;
            }
            else
            {
                mover.StartMovementAction(nextWaypointPos);
                if (isAtWaypoint)
                {
                    CycleWaypoint();
                    timeLeftAtWaypoint = waypointDwellTime;
                }
            }
        }

        bool isAtWaypoint => Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance;
        Vector3 nextWaypointPos => patrolPath.transform.GetChild(nextWaypointIndex).position;

        int IRaycastable.priority => 6;

        CursorType IRaycastable.cursor => CursorType.Attack;

        private void CycleWaypoint()
        {
            nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
        }

        void OnDrawGizmos()
        {
            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }

        bool IRaycastable.HandleRaycast(PlayerControl playerControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerControl.GetComponent<SpecialAbilities>().RequestSpecialAbility(0, gameObject);
            }
            if (Input.GetMouseButtonDown(1))
            {
                playerControl.GetComponent<SpecialAbilities>().RequestSpecialAbility(1, gameObject);
            }

            return true;
        }
    }
}