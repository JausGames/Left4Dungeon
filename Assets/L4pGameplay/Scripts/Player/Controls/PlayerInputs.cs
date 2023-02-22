using UnityEngine;
using UnityEngine.InputSystem;
using L4P.Gameplay.Player;
using static UnityEngine.InputSystem.InputAction;
using L4P.Gameplay.Player.TopDown;
using System.Collections;
using System.Collections.Generic;
using System;
using L4P.Gameplay.Player.Animations;

namespace L4P.Gameplay.Player.Controls
{

    [RequireComponent(typeof(Controls.InputManager))]
    public class PlayerInputs : MonoBehaviour
    {
        Queue<ActionItem> inputBuffer = new Queue<ActionItem>();
        [SerializeField] List<ActionItem> list = new List<ActionItem>();
        IPlayerController motor = null;
        IPlayerCombat combat = null;
        PlayerAnimatorController animator = null;

        [SerializeField] bool alternativeUse = false;
        [SerializeField] ActionItem currentAction = null;

        public void Start()
        {
            /*Debug.Log("Network Informations : IsOwner " + IsOwner);
            if (!IsOwner) return;*/

            motor = GetComponent<IPlayerController>();
            combat = GetComponent<IPlayerCombat>();
            animator = GetComponent<PlayerAnimatorController>();

            InputManager.Controls.Gameplay.Move.performed += ctx => OnMove(ctx.ReadValue<Vector2>());
            InputManager.Controls.Gameplay.Move.canceled += _ => OnMove(Vector2.zero);

            InputManager.Controls.Gameplay.Sprint.performed += _ => OnSprint(true);
            InputManager.Controls.Gameplay.Sprint.canceled += _ => OnSprint(false);

            InputManager.Controls.Gameplay.UseLeft.performed += _ => OnUseWeapon(true, AttackType.Left);
            InputManager.Controls.Gameplay.UseLeft.canceled += _ => OnUseWeapon(false, AttackType.Left);

            InputManager.Controls.Gameplay.UseRight.performed += _ => OnUseWeapon(true, alternativeUse ? AttackType.Strong : AttackType.Right);
            InputManager.Controls.Gameplay.UseRight.canceled += _ => OnUseWeapon(false, alternativeUse ? AttackType.Strong : AttackType.Right);

            InputManager.Controls.Gameplay.KeyboardAlternative.performed += _ => alternativeUse = true;
            InputManager.Controls.Gameplay.KeyboardAlternative.canceled += _ => alternativeUse = false;
        }
        public void OnMove(Vector2 context)
        {
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //if (motor == null || !IsOwner) return;
            motor.SetMove(context);
        }
        public void OnSprint(bool context)
        {
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //if (motor == null || !IsOwner) return;

            //motor.SetSprint(context);

            inputBuffer.Enqueue(
                new ActionItem(
                    ActionItem.InputAction.Sprint, 
                    () => { motor.SetSprint(context); },
                    (currentAction) => {
                        return (
                            animator.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("idle")
                            || animator.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("walk")
                            || animator.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("run")
                        ) && !animator.Animator.GetCurrentAnimatorClipInfo(1)[0].clip.name.Contains("hit reaction");
                    },
                    .2f)
                );
        }
        public void OnUseWeapon(bool context, AttackType type)
        {

            Debug.Log("caca : performed = " + context);
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //if (motor == null || !IsOwner) return;

            //combat.UseWeapon(context, type);

            inputBuffer.Enqueue(
                new ActionItem(
                    ActionItem.InputAction.Attack,
                    () => { combat.UseWeapon(context, type); },
                    (currentAction) => {
                        return (
                            (
                                animator.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("idle")
                                || animator.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("walk")
                                || animator.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("run")
                            )
                            || 
                            (
                                animator.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("attack") 
                                && animator.Comboable 
                                && !animator.Combo
                            )
                        ) && !animator.Animator.GetCurrentAnimatorClipInfo(1)[0].clip.name.Contains("hit reaction");
                    },
                    .2f)
                );
        }

        private void Update()
        {
            list = new List<ActionItem>(inputBuffer);
            if(inputBuffer.Count > 0)
            {
                var action = inputBuffer.Peek();
                if (!action.CheckIfValid()) inputBuffer.Dequeue();
                else if (action.CanTransit(currentAction))
                {
                    currentAction = inputBuffer.Dequeue();
                    currentAction.action();
                }
            }
        }
    }

    [Serializable]
    public class ActionItem
    {
        [HideInInspector] public enum InputAction { Sprint, Attack };
        public InputAction actionType;
        [HideInInspector] public Action action;
        [HideInInspector] public Func<ActionItem, bool> checkTransitionAction;
        [HideInInspector] public float Timestamp;

        public static float TimeBeforeActionsExpire = .5f;

        //Constructor
        public ActionItem(InputAction ia, Action action, Func<ActionItem, bool> checkTransition, float maxDelay)
        {
            actionType = ia;
            this.action = action;
            Timestamp = Time.time + maxDelay;
            checkTransitionAction = checkTransition;
        }

        //returns true if this action hasn't expired due to the timestamp
        public bool CheckIfValid()
        {
            bool returnValue = false;
            if (Timestamp + TimeBeforeActionsExpire >= Time.time)
            {
                returnValue = true;
            }
            return returnValue;
        }

        public bool CanTransit(ActionItem currentAction)
        {
            return checkTransitionAction(currentAction);
        }

    }
}