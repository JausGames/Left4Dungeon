using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace L4P.Gameplay.Player.Animations
{
    public class PlayerAnimatorEvent : MonoBehaviour
    {
        UnityEvent leftActivateEvent = new UnityEvent();
        UnityEvent leftDeactivateEvent = new UnityEvent();
        UnityEvent rightActivateEvent = new UnityEvent();
        UnityEvent rightDeactivateEvent = new UnityEvent();

        public UnityEvent LeftActivateEvent { get => leftActivateEvent; }
        public UnityEvent LeftDeactivateEvent { get => leftDeactivateEvent; }
        public UnityEvent RightActivateEvent { get => rightActivateEvent; }
        public UnityEvent RightDeactivateEvent { get => rightDeactivateEvent; }

        public void OnActivateRightHand() => rightActivateEvent.Invoke();
        public void OnDeactivateRightHand() => rightDeactivateEvent.Invoke();
        public void OnActivateLeftHand() => leftActivateEvent.Invoke();
        public void OnDeactivateLeftHand() => leftDeactivateEvent.Invoke();
    }
}
