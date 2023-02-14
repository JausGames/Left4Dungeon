using L4P.Gameplay.Player.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRootMotionBehaviour : StateMachineBehaviour
{
    [SerializeField] AnimationClip clip;
    [SerializeField] float speed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var controller = animator.GetComponentInParent<PlayerAnimatorController>();
        var animatorEvent = animator.GetComponent<PlayerAnimatorEvent>();
        controller.OnAnimationChange(clip, speed);
        controller.SetRootMotion(true);
        animatorEvent.IsAttacking.Invoke();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetCurrentAnimatorClipInfo(0)[layerIndex].clip.name == clip.name) return;
        var controller = animator.GetComponentInParent<PlayerAnimatorController>();
        var animatorEvent = animator.GetComponent<PlayerAnimatorEvent>();
        controller.SetRootMotion(false);
        animatorEvent.IsNotAttacking.Invoke();


    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
