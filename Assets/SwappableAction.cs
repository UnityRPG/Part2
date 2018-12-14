using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SwappableAction : StateMachineBehaviour
{

    [SerializeField] string conditionName;
    [SerializeField] string animationName;

    public bool hasQueueActions => _actionQueue.Count > 0;

    public void ReplaceNextAction(Animator animator, AnimationClip clip, System.Action callback)
    {
        var attack = new Action();
        attack.clip = clip;
        attack.callback = callback;
        _actionQueue.Clear();
        _actionQueue.Enqueue(attack);
        animator.SetBool(conditionName, hasQueueActions);
    }

    private Queue<Action> _actionQueue = new Queue<Action>(1);

    struct Action
    {
        public AnimationClip clip;
        public System.Action callback;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasQueueActions)
        {
            var animatorOverrideController =  animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverrideController == null)
            {
                animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
                animator.runtimeAnimatorController = animatorOverrideController;
            }
            var animationOverride = _actionQueue.Dequeue();

            animatorOverrideController[animationName] = animationOverride.clip;

            animationOverride.callback();
        }


        animator.SetBool(conditionName, hasQueueActions);
    }
}
