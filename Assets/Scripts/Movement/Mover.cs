using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Core;
using System.Collections.Generic;

namespace RPG.Movement
{
    [SelectionBase]
    public class Mover : MonoBehaviour, ISaveable, ISchedulableAction
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
        [SerializeField] float maxNavDistance = 20f;

        [Header("Nav Mesh Agent")]
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;
        [SerializeField] float navMeshAgentStoppingDistance = 1.3f;

        [Header("Saving")]
        [SerializeField] bool shouldLoadOrSaveLocation = true;


        // private instance variables for state
        float turnAmount;
        float forwardAmount;
				
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

            // rigidBody = gameObject.AddComponent<Rigidbody>();
            // rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;

            actionScheduler = gameObject.AddComponent<ActionScheduler>();
            actionScheduler.animatorOverrideController = animatorOverrideController;
            actionScheduler.characterAvatar = characterAvatar;
            actionScheduler.animationSpeedMultiplier = animationSpeedMultiplier;

            animator = GetComponent<Animator>();

            navMeshAgent = GetComponent<NavMeshAgent>();
            if (!navMeshAgent)
            {
                navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
                navMeshAgent.speed = navMeshAgentSteeringSpeed;
                navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;
                navMeshAgent.speed = moveSpeedMultiplier;
                navMeshAgent.autoBraking = true;
            }
            // Position is done through root motion
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
        }

        void Update()
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && actionScheduler.IsRunningAction(this))
            {
                StopMoving();
            } 

            UpdateAnimator();

            // Pull agent to rootmotion location
            navMeshAgent.nextPosition = transform.position;
        }

        public void StartMovementAction(Vector3 worldPos)
        {
            if (gameObject.tag == "Player") print("Dest:" + worldPos);
            if (gameObject.tag == "Player")
                print("starting to move");

            navMeshAgent.destination = worldPos;

            actionScheduler.QueueAction(this);
        }

        void ISchedulableAction.Start()
        {
            StartMoving();
        }

        void ISchedulableAction.RequestCancel()
        {
            StopMoving();
        }

        public void StopMovementAction()
        {
            StopMoving();
        }

        public void StartMoving(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;

            StartMoving();
        }

        public void StopMoving()
        {
            navMeshAgent.isStopped = true;
            actionScheduler.FinishAction(this);
        }

        private void StartMoving()
        {
            if (navMeshAgent.isStopped)
            {
                animator.SetTrigger("Move");
            }
            navMeshAgent.isStopped = false;
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        void UpdateAnimator()
        {
            actionScheduler.forwardAmountRequest = 0;

            Vector3 horizontalVelocity = navMeshAgent.velocity;
            if (horizontalVelocity.magnitude > Mathf.Epsilon)
            {
                Vector3 localVelocity = transform.InverseTransformVector(navMeshAgent.velocity);
                actionScheduler.forwardAmountRequest =  localVelocity.z / animationSpeedMultiplier;
                horizontalVelocity.y = 0;
                transform.LookAt(transform.position + horizontalVelocity);
            }
        }

        private void OnAnimatorMove() {
            var position = animator.rootPosition;
            position.y = navMeshAgent.nextPosition.y;
            transform.position = position;
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