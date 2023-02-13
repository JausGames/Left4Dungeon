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
            var victim = other.GetComponent<Hitable>() ? other.GetComponent<Hitable>() : other.GetComponentInParent<Hitable>();
            if (victim == weapon.Owner || touchedList.Contains(victim)) return;

            victim.TakeDamage(weapon.Stats, (victim.transform.position - weapon.Owner.transform.position).normalized);
            touchedList.Add(victim);

            Debug.Log("WeaponTrigger, OnTriggerEnter : hitable " + victim.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;


        if (other.GetComponent<Hitable>() || other.GetComponentInParent<Hitable>())
        {
            var victim = other.GetComponent<Hitable>() ? other.GetComponent<Hitable>() : other.GetComponentInParent<Hitable>();
            if (victim == weapon.Owner || touchedList.Contains(victim)) return;

            victim.TakeDamage(weapon.Stats, (victim.transform.position - weapon.Owner.transform.position).normalized);
            touchedList.Add(victim);

            Debug.Log("WeaponTrigger, OnTriggerStay : hitable " + victim.gameObject);
        }
    }

    public void ResetSwing()
    {
        touchedList.Clear();
    }
}