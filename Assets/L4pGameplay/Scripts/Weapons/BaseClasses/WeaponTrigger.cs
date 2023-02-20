using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L4P.Gameplay.Enemy;
using L4P.Gameplay.Weapons;
using System;

public class WeaponTrigger : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] List<Hitable> touchedList;
    [SerializeField] List<ParticleSystem> particles;
    [SerializeField] bool isActive = false;

    private float damageMultiplier = 1f;
    private float knockbackMultiplier = 1f;

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
        TryHitCollider(other);
    }
    private void OnTriggerStay(Collider other)
    {
        TryHitCollider(other);
    }

    private void TryHitCollider(Collider other)
    {
        if (!isActive) return;


        if (other.GetComponent<Hitable>() || other.GetComponentInParent<Hitable>())
        {
            var victim = other.GetComponent<Hitable>() ? other.GetComponent<Hitable>() : other.GetComponentInParent<Hitable>();
            if (victim == weapon.Owner || touchedList.Contains(victim)) return;

            var currStats = new WeaponStat(weapon.Stats);
            currStats.damage *= damageMultiplier;
            currStats.knockback *= knockbackMultiplier;

            victim.TakeDamage(currStats, (victim.transform.position - weapon.Owner.transform.position).normalized);
            touchedList.Add(victim);

            Debug.Log("WeaponTrigger, OnTriggerStay : hitable " + victim.gameObject);
        }
    }

    internal void ChangeStatsMultiplier(float damageMultiplier, float knockdownMultiplier)
    {
        Debug.Log("WeaponTrigger, ChangeStatsMultiplier : damageMultiplier " + damageMultiplier);
        this.damageMultiplier = damageMultiplier;
        this.knockbackMultiplier = knockdownMultiplier;
    }

    public void ResetSwing()
    {
        touchedList.Clear();
    }
}