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

        private Rigidbody body;
        private bool getHit;
        private bool upperBodyLayerOn = false;

        public bool Comboable { get => comboable; set => comboable = value; }
        public bool Combo { get => animator.GetBool("Combo"); }
        public bool GetHit1 { get => getHit; set
            {

                animator.SetBool("GetHit", value);
                getHit = value;
            }
        }
        

        public bool UpperBodyLayerOn { get => upperBodyLayerOn; set => upperBodyLayerOn = value; }

        internal void SetComboable(bool v)
        {
            Comboable = v;
        }

        internal void SetCombo(bool v)
        {
            Debug.Log("PlayerAnimatorController : combo = " + v);
            if (v && !Comboable) return;
            animator.SetBool("Combo", v);
        }

        PlayerAnimatorEvent animatorEvent;
        private bool isAttacking;
        [SerializeField] private bool root;

        internal void Die()
        {
            animator.SetTrigger("Die");
        }


        public void SetMove(Vector2 move) => this.move = move;
        private void Awake()
        {
            body = GetComponentInChildren<Rigidbody>();
            animatorEvent = GetComponentInChildren<PlayerAnimatorEvent>();

            animatorEvent.IsAttacking.AddListener(delegate { isAttacking = true; });
            animatorEvent.IsNotAttacking.AddListener(delegate { isAttacking = false; });

            animatorEvent.IsComboable.AddListener(delegate { Comboable = true; });
            animatorEvent.IsNotComboable.AddListener(delegate { Comboable = false; });

            animatorEvent.StopComboEvent.AddListener(delegate { if (!Combo) animator.SetTrigger("StopCombo"); });


            Debug.Log("AnimHash : StrongR = " + Animator.StringToHash("StrongR"));
            Debug.Log("AnimHash : Attack = " + Animator.StringToHash("Attack"));
            Debug.Log("AnimHash : AttackL = " + Animator.StringToHash("AttackL"));
            Debug.Log("AnimHash : AttackR = " + Animator.StringToHash("AttackR"));
            Debug.Log("AnimHash : CastL = " + Animator.StringToHash("CastL"));
            Debug.Log("AnimHash : CastR = " + Animator.StringToHash("CastR"));
        }

        public void SetRootMotion(bool value)
        {
            root = value;
        }

        private void Update()
        {
            if (root)
            {

                var curve = currentRootMotionData.curve;
                var currentTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                Debug.Log("IsComboableBehaviour : currentTime = " + currentTime);

                body.transform.position += currentRootMotionData.speed * body.transform.forward * (curve.Evaluate(currentTime * currentRootMotionData.length) - curve.Evaluate((currentTime * currentRootMotionData.length - Time.deltaTime)));
            }
            
            if(upperBodyLayerOn || GetHit1)
            {
                ManageBooleanLayer(true, 1);
            }
            /*else if (GetHit1 && hitTime + 1f <= Time.time)
            {
                GetHit1 = false;
            }*/
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

        internal void Roll()
        {
            animator.SetTrigger("Roll");
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
            if (GetHit1) return;
            if (isRight) castR = performed;
            else castL = performed;

            animator.SetBool(animAttackBoolId, performed);
        }

        public void SetAttackAnimationTrigger(bool performed, int animAttackTriggerId = 1080829965)
        {
            if (GetHit1) return;
            /*if (Comboable && performed)
            {
                animator.SetBool("Combo", true);
            }*/
            //else 
            if (performed && !isAttacking)
            {
                if (performed)
                    animator.SetTrigger(animAttackTriggerId);
                else
                    animator.ResetTrigger(animAttackTriggerId);
            }
        }
        internal void GetHit()
        {
            GetHit1 = true;
            animator.SetBool("Combo", false);
        }

        internal float GetZRootMotionVelocity()
        {
            return animator.GetFloat("Zvelocity");
        }
    }

    class AnimationClipRootMotionData
    {
        public AnimationCurve curve;
        public float length;
        public float speed;
    }
}
