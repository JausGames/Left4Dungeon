using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L4P.Gameplay.Player.Animations
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] float blendSpeed = 4f;
        [SerializeField] bool castL = false;
        [SerializeField] bool castR = false;

        Vector2 move;

        public void SetMove(Vector2 move) => this.move = move;
        private void Awake()
        {

            Debug.Log("AttackL = " + Animator.StringToHash("AttackL"));
            Debug.Log("AttackR = " + Animator.StringToHash("AttackR"));
            Debug.Log("CastL = " + Animator.StringToHash("CastL"));
            Debug.Log("CastR = " + Animator.StringToHash("CastR"));
        }
        private void Update()
        {
            var newX = Mathf.MoveTowards(animator.GetFloat("MoveX"), move.x, Time.deltaTime * blendSpeed);
            var newY = Mathf.MoveTowards(animator.GetFloat("MoveY"), move.y, Time.deltaTime * blendSpeed);
            animator.SetFloat("MoveX", newX);
            animator.SetFloat("MoveY", newY);

            ManageBooleanLayer((castL && !castR) || (!castL && castR), 1);
            ManageBooleanLayer(castL && castR, 2);
            ManageBooleanLayer(castR && castL, 3);

        }

        private void ManageBooleanLayer(bool cast, int layerId)
        {
            if (cast && animator.GetLayerWeight(layerId) != 1f)
            {
                var weight = Mathf.MoveTowards(animator.GetLayerWeight(layerId), 1f, Time.deltaTime * blendSpeed);
                animator.SetLayerWeight(layerId, weight);
            }
            else if (!cast && animator.GetLayerWeight(layerId) != 0f)
            {
                var weight = Mathf.MoveTowards(animator.GetLayerWeight(layerId), 0f, Time.deltaTime * blendSpeed);
                animator.SetLayerWeight(layerId, weight);
            }
        }

        internal void SetAttackAnimationBool(bool performed, bool isRight ,int animAttackBoolId = 1080829965)
        {
            if (isRight) castR = performed;
            else castL = performed;
            animator.SetBool(animAttackBoolId, performed);
        }

        public void SetAttackAnimationTrigger(bool performed, int animAttackTriggerId = 1080829965)
        {
            if(performed)
                animator.SetTrigger(animAttackTriggerId);
            else
                animator.ResetTrigger(animAttackTriggerId);
        }
    }
}
