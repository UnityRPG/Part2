using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ActionQueue : StateMachineBehaviour
{

    [SerializeField] string conditionName;
    [SerializeField] string animationName;

    public bool hasQueueActions => _actionQueue.Count > 0;

    public void QueueAction(Animator animator, AnimationClip clip, System.Action callback)
    {
        var attack = new Action();
        attack.clip = clip;
        attack.callback = callback;
        _actionQueue.Enqueue(attack);
        animator.SetBool(conditionName, hasQueueActions);
    }

    private Queue<Action> _actionQueue = new Queue<Action>();

    struct Action
    {
        public AnimationClip clip;
        public System.Action callback;
    }

    private Action _actionToReset;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_actionToReset.clip == null && _actionQueue.Count > 0)
        {
            _actionToReset = new Action();

            var animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            _actionToReset.clip = animatorOverrideController[animationName];

            var animationOverride = _actionQueue.Dequeue();
            _actionToReset.callback = animationOverride.callback;

            animatorOverrideController[animationName] = animationOverride.clip;
            animator.runtimeAnimatorController = animatorOverrideController;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(conditionName, hasQueueActions);

        if (_actionToReset.clip != null)
        {
            var animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animatorOverrideController[animationName] = _actionToReset.clip;
            animator.runtimeAnimatorController = animatorOverrideController;
            _actionToReset.callback();
            _actionToReset.clip = null;
        }
    }
}
