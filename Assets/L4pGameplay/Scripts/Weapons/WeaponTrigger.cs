using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L4P.Gameplay.Enemy;
using L4P.Gameplay.Weapons;

public class WeaponTrigger : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] List<Hitable> touchedList;
    [SerializeField] List<ParticleSystem> particles;
    [SerializeField] bool isActive = false;

    public bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            touchedList.Clear();
            if (value)
                foreach (ParticleSystem prtcl in particles)
                {
                    if (prtcl.isPaused || prtcl.isStopped) prtcl.Play();
                    var em = prtcl.emission;
                    em.enabled = true;
                }
            else
                foreach (ParticleSystem prtcl in particles)
                {
                    var em = prtcl.emission;
                    em.enabled = false;
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;


        if (other.GetComponent<Hitable>() || other.GetComponentInParent<Hitable>())
        {
            var minion = other.GetComponent<Hitable>() ? other.GetComponent<Hitable>() : other.GetComponentInParent<Hitable>();
            if (minion == weapon.Owner || touchedList.Contains(minion)) return;

            minion.TakeDamage(weapon.Stats.damage);
            touchedList.Add(minion);

            Debug.Log("WeaponTrigger, OnTriggerEnter : hitable " + minion.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;


        if (other.GetComponent<Hitable>() || other.GetComponentInParent<Hitable>())
        {
            var minion = other.GetComponent<Hitable>() ? other.GetComponent<Hitable>() : other.GetComponentInParent<Hitable>();
            if (minion == weapon.Owner || touchedList.Contains(minion)) return;

            minion.TakeDamage(weapon.Stats.damage);
            touchedList.Add(minion);

            Debug.Log("WeaponTrigger, OnTriggerEnter : hitable " + minion.gameObject);
        }
    }

    public void ResetSwing()
    {
        touchedList.Clear();
    }
}