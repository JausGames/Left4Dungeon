using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace L4P.Gameplay.Player.Animations
{
    public class PlayerAnimatorEvent : MonoBehaviour
    {
        UnityEvent isAttacking = new UnityEvent();
        UnityEvent isNotAttacking = new UnityEvent();

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
        public UnityEvent IsNotAttacking { get => isNotAttacking; set => isNotAttacking = value; }
        public UnityEvent RootMotionActivateEvent { get => rootMotionActivateEvent; set => rootMotionActivateEvent = value; }
        public UnityEvent RootMotionDeactivateEvent { get => rootMotionDeactivateEvent; set => rootMotionDeactivateEvent = value; }

        public void OnIsAttacking() => IsAttacking.Invoke();
        public void OnIsNotAttacking() => IsNotAttacking.Invoke();

        public void OnActivateRightHand() => rightActivateEvent.Invoke();
        public void OnDeactivateRightHand() => rightDeactivateEvent.Invoke();
        public void OnActivateLeftHand() => leftActivateEvent.Invoke();
        public void OnDeactivateLeftHand() => leftDeactivateEvent.Invoke();

        public void OnActivateRootMotion() => RootMotionActivateEvent.Invoke();
        public void OnDeactivateRootMotion() => RootMotionDeactivateEvent.Invoke();
    }
}
