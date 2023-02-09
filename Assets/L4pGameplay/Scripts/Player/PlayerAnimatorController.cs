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
        [SerializeField] bool cast = false;

        Vector2 move;

        public void SetMove(Vector2 move) => this.move = move;
        private void Update()
        {
            var newX = Mathf.MoveTowards(animator.GetFloat("MoveX"), move.x, Time.deltaTime * blendSpeed);
            var newY = Mathf.MoveTowards(animator.GetFloat("MoveY"), move.y, Time.deltaTime * blendSpeed);
            animator.SetFloat("MoveX", newX);
            animator.SetFloat("MoveY", newY);

            if(cast && animator.GetLayerWeight(1) != 1f)
            {
                var weight = Mathf.MoveTowards(animator.GetLayerWeight(1), 1f, Time.deltaTime * blendSpeed);
                animator.SetLayerWeight(1, weight);
            }
            else if (!cast && animator.GetLayerWeight(1) != 0f)
            {
                var weight = Mathf.MoveTowards(animator.GetLayerWeight(1), 0f, Time.deltaTime * blendSpeed);
                animator.SetLayerWeight(1, weight);
            }
        }

        internal void SetAttackAnimation(bool performed)
        {
            cast = performed;
            animator.SetBool("Cast", performed);
        }
    }
}
