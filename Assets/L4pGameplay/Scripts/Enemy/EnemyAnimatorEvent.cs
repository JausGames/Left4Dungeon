using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimatorEvent : MonoBehaviour
{
    UnityEvent isNotAttackingEvent = new UnityEvent();
    UnityEvent hitEvent = new UnityEvent();

    public UnityEvent IsNotAttackingEvent { get => isNotAttackingEvent; set => isNotAttackingEvent = value; }
    public UnityEvent HitEvent { get => hitEvent; set => hitEvent = value; }
    public void OnIsNotAttacking() => isNotAttackingEvent.Invoke();
    public void OnHit() => hitEvent.Invoke();
}
