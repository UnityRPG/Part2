using UnityEngine;
using System;

namespace RPG.Characters
{
    public class ActionScheduler : MonoBehaviour {
        
        private Animator animator;

        private void Awake() {
            animator = gameObject.AddComponent<Animator>();
            animator.applyRootMotion = true;
        }

        public AnimatorOverrideController animatorOverrideController
        {
            set
            {
                animator.runtimeAnimatorController = value;
            }
        }

        public Avatar characterAvatar
        {
            set
            {
                animator.avatar = value;
            }
        }

        public float forwardAmountRequest
        {
            set
            {
                animator.SetFloat("Forward", value, 0.1f, Time.deltaTime);
            }
        }

        public float turnAmountRequest
        {
            set
            {
                animator.SetFloat("Turn", value, 0.1f, Time.deltaTime);
            }
        }

        public float animationSpeedMultiplier
        {
            set
            {
                animator.speed = value;
            }
        }

        public Vector3 deltaIKPosition => animator.deltaPosition;

        public float animSpeedMultiplier => animator.speed;

        public event Action<Vector3, Quaternion> onMove;

        public bool isMoving => animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded");


        void OnAnimatorMove()
        {
            onMove(animator.deltaPosition, animator.deltaRotation);     
        }
    }
}