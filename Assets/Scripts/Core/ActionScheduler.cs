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
            // animator.applyRootMotion = true;
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
                animator.SetFloat("Forward", value);
            }
        }

        public float turnAmountRequest
        {
            set
            {
                animator.SetFloat("Turn", value);
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

        private SchedulableAction runningAction = null;
        private SchedulableAction queuedAction = null;

        public void QueueAction(SchedulableAction action)
        {
            action.OnFinish += () => FinishAction(action);

            queuedAction = action;

            if (runningAction != null && runningAction.isInterruptable)
            {
                runningAction.Cancel();
                runningAction = null;
            }

            ProgressQueue();
        }

        private void FinishAction(SchedulableAction action)
        {
            if (runningAction == action)
            {
                runningAction = null;
                ProgressQueue();
            }
        }

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