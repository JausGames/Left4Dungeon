using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace L4P.Gameplay.Player.Animations
{
    public class PlayerAnimatorEvent : MonoBehaviour
    {
        UnityEvent isAttacking = new UnityEvent();
        UnityEvent isMobileAttacking = new UnityEvent();
        UnityEvent isNotAttacking = new UnityEvent();
        UnityEvent isMobileNotAttacking = new UnityEvent();

        UnityEvent resetComboEvent = new UnityEvent();

        UnityEvent isComboable = new UnityEvent();
        UnityEvent isNotComboable = new UnityEvent();

        UnityEvent leftActivateEvent = new UnityEvent();
        UnityEvent leftDeactivateEvent = new UnityEvent();
        UnityEvent rightActivateEvent = new UnityEvent();
        UnityEvent rightDeactivateEvent = new UnityEvent();

        UnityEvent rootMotionActivateEvent = new UnityEvent();
        UnityEvent rootMotionDeactivateEvent = new UnityEvent();

        public UnityEvent LeftActivateEvent { get => leftActivateEvent; }
        public UnityEvent LeftDeactivateEvent { get => leftDeactivateEvent; }
        public UnityEvent RightActivateEvent { get => rightActivateEvent; }
        public UnityEvent RightDeactivateEvent { get => rightDeactivateEvent; }
        public UnityEvent IsAttacking { get => isAttacking; set => isAttacking = value; }
        public UnityEvent IsMobileAttacking { get => isMobileAttacking; set => isMobileAttacking = value; }
        public UnityEvent IsNotAttacking { get => isNotAttacking; set => isNotAttacking = value; }
        public UnityEvent IsMobileNotAttacking { get => isMobileNotAttacking; set => isMobileNotAttacking = value; }
        public UnityEvent RootMotionActivateEvent { get => rootMotionActivateEvent; set => rootMotionActivateEvent = value; }
        public UnityEvent RootMotionDeactivateEvent { get => rootMotionDeactivateEvent; set => rootMotionDeactivateEvent = value; }
        public UnityEvent ResetComboEvent { get => resetComboEvent; set => resetComboEvent = value; }
        public UnityEvent IsComboable { get => isComboable; set => isComboable = value; }
        public UnityEvent IsNotComboable { get => isNotComboable; set => isNotComboable = value; }

        public void OnIsAttacking() => IsAttacking.Invoke();
        public void OnIsMobileAttacking() => IsMobileAttacking.Invoke();
        public void OnIsNotAttacking() => IsNotAttacking.Invoke();
        public void OnIsMobileNotAttacking() => IsMobileNotAttacking.Invoke();

        public void OnResetCombo() => resetComboEvent.Invoke();
        public void OnIsComboable() => isComboable.Invoke();
        public void OnIsNotComboable() => IsNotComboable.Invoke();

        public void OnActivateRightHand() => rightActivateEvent.Invoke();
        public void OnDeactivateRightHand() => rightDeactivateEvent.Invoke();
        public void OnActivateLeftHand() => leftActivateEvent.Invoke();
        public void OnDeactivateLeftHand() => leftDeactivateEvent.Invoke();

        public void OnActivateRootMotion() => RootMotionActivateEvent.Invoke();
        public void OnDeactivateRootMotion() => RootMotionDeactivateEvent.Invoke();
    }
}
