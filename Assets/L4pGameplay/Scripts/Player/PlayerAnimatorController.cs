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
        [SerializeField] bool comboable = false;

        Vector2 move;
        [SerializeField] AnimationCurve rootMotionCurve;
        [SerializeField] AnimationClipRootMotionData currentRootMotionData = new AnimationClipRootMotionData();

        internal void SetComboable(bool v)
        {
            comboable = v;
        }

        internal void SetCombo(bool v)
        {
            if (v && !comboable) return;
            animator.SetBool("Combo", v);
        }

        PlayerAnimatorEvent animatorEvent;
        private bool isAttacking;
        [SerializeField] private bool root;

        internal void Die()
        {
            animator.SetTrigger("Die");
        }

        private float timeRoot;
        private Rigidbody body;
        private float hitTime;
        private bool getHit;

        public void SetMove(Vector2 move) => this.move = move;
        private void Awake()
        {
            body = GetComponentInChildren<Rigidbody>();
            animatorEvent = GetComponentInChildren<PlayerAnimatorEvent>();

            animatorEvent.IsAttacking.AddListener(delegate { isAttacking = true; });
            animatorEvent.IsNotAttacking.AddListener(delegate { isAttacking = false; });

            animatorEvent.IsComboable.AddListener(delegate { comboable = true; });
            animatorEvent.IsNotComboable.AddListener(delegate { comboable = false; });


            Debug.Log("AnimHash : Attack = " + Animator.StringToHash("Attack"));
            Debug.Log("AnimHash : AttackL = " + Animator.StringToHash("AttackL"));
            Debug.Log("AnimHash : AttackR = " + Animator.StringToHash("AttackR"));
            Debug.Log("AnimHash : CastL = " + Animator.StringToHash("CastL"));
            Debug.Log("AnimHash : CastR = " + Animator.StringToHash("CastR"));
        }

        public void SetRootMotion(bool value)
        {
            root = value;
            timeRoot = Time.time;
        }

        private void Update()
        {
            if (root)
            {
                var speed = currentRootMotionData.speed;
                var curve = currentRootMotionData.curve;
                var passedTime = Time.time - timeRoot;


                body.transform.position += body.transform.forward * (curve.Evaluate(passedTime * speed) - curve.Evaluate((passedTime - Time.deltaTime) * speed));
            }

            if(getHit && hitTime + 1f > Time.time)
            {
                ManageBooleanLayer(true, 1);
            }
            else if(isAttacking)
            {
                if (animator.GetLayerWeight(1) > 0f) ManageBooleanLayer(false, 1);
                if (animator.GetLayerWeight(2) > 0f) ManageBooleanLayer(false, 2);
                if (animator.GetLayerWeight(3) > 0f) ManageBooleanLayer(false, 3);
            }
            else
            {
                ManageBooleanLayer((castL && !castR) || (!castL && castR), 1);
                ManageBooleanLayer(castL && castR, 2);
                ManageBooleanLayer(castR && castL, 3);
            }

            SetMovementAnimation();
        }

        private void SetMovementAnimation()
        {
            var newX = Mathf.MoveTowards(animator.GetFloat("MoveX"), move.x, Time.deltaTime * blendSpeed);
            var newY = Mathf.MoveTowards(animator.GetFloat("MoveY"), move.y, Time.deltaTime * blendSpeed);
            animator.SetFloat("MoveX", newX);
            animator.SetFloat("MoveY", newY);
        }

        public void OnAnimationChange(AnimationClip clip, float speed)
        {
            //var clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;

            var curveBindings = UnityEditor.AnimationUtility.GetCurveBindings(clip);

            //var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            
            currentRootMotionData.length = clip.length;
            currentRootMotionData.speed = speed;


                foreach (var curveBinding in curveBindings)
                {
                    //if (curveBinding.propertyName.Contains("RootT.x")) boneCurves[0].curve[0] = UnityEditor.AnimationUtility.GetEditorCurve(clip, curveBinding);
                    //if (curveBinding.propertyName.Contains("RootT.y")) boneCurves[0].curve[1] = UnityEditor.AnimationUtility.GetEditorCurve(clip, curveBinding);
                    if (curveBinding.propertyName.Contains("RootT.z")) currentRootMotionData.curve = UnityEditor.AnimationUtility.GetEditorCurve(clip, curveBinding);
                    //if (curveBinding.propertyName.Contains("RootQ.x")) boneCurves[0].curve[3] = UnityEditor.AnimationUtility.GetEditorCurve(clip, curveBinding);
                    //if (curveBinding.propertyName.Contains("RootQ.y")) boneCurves[0].curve[4] = UnityEditor.AnimationUtility.GetEditorCurve(clip, curveBinding);
                    //if (curveBinding.propertyName.Contains("RootQ.z")) boneCurves[0].curve[5] = UnityEditor.AnimationUtility.GetEditorCurve(clip, curveBinding);
                    //if (curveBinding.propertyName.Contains("RootQ.w")) boneCurves[0].curve[6] = UnityEditor.AnimationUtility.GetEditorCurve(clip, curveBinding);
                }
        }
        public AnimationClip FindAnimation(string name)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == name)
                {
                    return clip;
                }
            }

            return null;
        }

        private void ManageBooleanLayer(bool activate, int layerId)
        {
            if (activate && animator.GetLayerWeight(layerId) != 1f)
            {
                var weight = Mathf.MoveTowards(animator.GetLayerWeight(layerId), 1f, Time.deltaTime * blendSpeed);
                animator.SetLayerWeight(layerId, weight);
            }
            else if (!activate && animator.GetLayerWeight(layerId) != 0f)
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
            if (comboable && performed)
            {
                animator.SetBool("Combo", true);
            }
            else
            {
                if (performed)
                    animator.SetTrigger(animAttackTriggerId);
                else
                    animator.ResetTrigger(animAttackTriggerId);
            }
        }
        internal void GetHit()
        {
            animator.SetTrigger("GetHit");
            hitTime = Time.time;
            getHit = true;
        }
    }

    class AnimationClipRootMotionData
    {
        public AnimationCurve curve;
        public float length;
        public float speed;
    }
}
