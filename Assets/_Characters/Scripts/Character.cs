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
				
		// cached references for readability
        NavMeshAgent navMeshAgent;
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

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;
            animator.applyRootMotion = true;

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
            else if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        public float GetAnimSpeedMultiplier()
        {
            return animator.speed;
        }

        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            if (gameObject.tag == "Player" && (forwardAmount != 0 || turnAmount != 0))
            {
                print("Move: "+movement);
                print("Forward: "+forwardAmount+" Turn: "+turnAmount);
            }
            ApplyExtraTurnRotation();
            UpdateAnimator();
            navMeshAgent.nextPosition = transform.position;
        }

        void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount * animatorForwardCap, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
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