using UnityEngine;
using UnityEngine.AI;
using RPG.Core.Saving;
using System.Collections.Generic;

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour, ISaveable
    {
        [Header("Animator")] [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;
        [SerializeField] [Range (.1f, 1f)] float animatorForwardCap = 1f;

        [Header("Audio")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.03f;

        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        [Header("Nav Mesh Agent")]
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;
        [SerializeField] float navMeshAgentStoppingDistance = 1.3f;

        [Header("Saving")]
        [SerializeField] bool shouldLoadOrSaveLocation = true;


        // private instance variables for state
        float turnAmount;
        float forwardAmount;
        SchedulableAction currentMovementAction;
				
		// cached references for readability
        NavMeshAgent navMeshAgent;
        ActionScheduler actionScheduler;
        Animator animator;
        Rigidbody rigidBody;


		// messages, then public methods, then private methods...
        void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;

            actionScheduler = gameObject.AddComponent<ActionScheduler>();
            actionScheduler.animatorOverrideController = animatorOverrideController;
            actionScheduler.characterAvatar = characterAvatar;
            actionScheduler.onMove += OnMove;

            animator = GetComponent<Animator>();

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = false;
            navMeshAgent.autoRepath = true;
        }

        void Update()
        {
            if (!navMeshAgent.isOnNavMesh)
            {
                Debug.LogError(gameObject.name + " uh oh this guy is not on the navmesh");
            }
            else if (!navMeshAgent.isStopped && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                if (currentMovementAction != null && currentMovementAction.isRunning)
                {
                    Move(navMeshAgent.desiredVelocity);
                } else
                {
                    currentMovementAction = new SchedulableAction(isInterruptable:true);
                    actionScheduler.QueueAction(currentMovementAction);
                }
            }
            else
            {
                Move(Vector3.zero);
                if (currentMovementAction != null)
                {
                    currentMovementAction.Finish();
                }
                ClearDestination();
            }
        }

        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
            navMeshAgent.isStopped = false;
        }

        public void ClearDestination()
        {
            navMeshAgent.isStopped = true;
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            RequestMovement();
        }

        void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction
            animator.SetBool("Move", movement.magnitude > 0);

            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void RequestMovement()
        {
            actionScheduler.forwardAmountRequest = forwardAmount * animatorForwardCap;
            actionScheduler.turnAmountRequest = turnAmount;
            actionScheduler.animationSpeedMultiplier = animationSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        void OnMove(Vector3 deltaIKPosition, Quaternion deltaIKRotation)
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (deltaIKPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;

                navMeshAgent.nextPosition = transform.position;
            }
        }

        private bool isAlive
        {
            get
            {
                var healthSystem = GetComponent<HealthSystem>();
                return !healthSystem || healthSystem.isAlive;
            }
        }

        public void CaptureState(IDictionary<string, object> state)
        {
            if (!shouldLoadOrSaveLocation) return;

            state["position"] = (SerializableVector3)GetComponent<Transform>().position;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            if (!shouldLoadOrSaveLocation) return;

            if (state.ContainsKey("position"))
            {
                gameObject.SetActive(false);
                GetComponent<Transform>().position = (SerializableVector3)state["position"];
                gameObject.SetActive(true);
            }
        }
    }
}