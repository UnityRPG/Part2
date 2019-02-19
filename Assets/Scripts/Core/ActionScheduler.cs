using UnityEngine;
using System;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour {
        
        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                animator = gameObject.AddComponent<Animator>();
            }
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
                animator.SetFloat("Forward", value, 0.2f, Time.deltaTime);
            }
        }

        public float turnAmountRequest
        {
            set
            {
                animator.SetFloat("Turn", value, 0.2f, Time.deltaTime);
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

        private ISchedulableAction runningAction = null;
        private ISchedulableAction queuedAction = null;

        public void QueueAction(ISchedulableAction action)
        {
            // Start hack
            var scheduleableAction = action as SchedulableAction;
            if (scheduleableAction != null)
            {
                scheduleableAction.scheduler = this;
            }
            // end hack

            if (IsRunningAction(action))
            {
                return;
            }

            queuedAction = action;

            if (runningAction != null)
            {
                runningAction.RequestCancel();
            } else
            {
                ProgressQueue();
            }
        }

        public void FinishAction(ISchedulableAction action)
        {
            if (runningAction == action)
            {
                runningAction = null;
                ProgressQueue();
            }
        }

        public bool IsRunningAction(ISchedulableAction action) => System.Object.ReferenceEquals(runningAction, action);

        private void ProgressQueue()
        {
            if (runningAction == null && queuedAction != null)
            {
                runningAction = queuedAction;
                queuedAction = null;
                runningAction.Start();
            }
        }
    }
}