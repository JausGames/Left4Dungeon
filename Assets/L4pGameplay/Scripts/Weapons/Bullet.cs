using UnityEngine;
using L4P.Gameplay.Enemy;

public class Bullet : MonoBehaviour
{
    private float range;
    private float damage;
    private LayerMask ennemyLayer;
    //where to add ?
    [SerializeField] private float speed;

    [SerializeField] ParticleSystem[] runningParticle;

    [SerializeField] ParticleSystem[] dieFromRangePrtcls;
    [SerializeField] ParticleSystem[] dieFromImpactPrtcls;

    private Vector3 startingPoint;
    internal void SetUp(float damage, float range, LayerMask ennemyLayer)
    {
        this.damage = damage;
        this.range = range;
        this.ennemyLayer = ennemyLayer;
        //speed = 6f;
    }
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward;
        startingPoint = transform.position;
    }
    private void Update()
    {
        if ((startingPoint - transform.position).magnitude > range) DieFromRange();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter, layer = " + LayerMask.LayerToName(collision.collider.gameObject.layer));
        if (ennemyLayer == (ennemyLayer | (1 << collision.collider.gameObject.layer)))
        {
            var victim = collision.collider.attachedRigidbody.GetComponent<Hitable>();
            if (victim)
                victim.TakeDamage(damage);
        }
        DieOnImpact();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter, layer = " + LayerMask.LayerToName(other.gameObject.layer));
        if (ennemyLayer == (ennemyLayer | (1 << other.gameObject.layer)))
        {
            var victim = other.attachedRigidbody.GetComponent<Hitable>();
            if (victim)
                victim.TakeDamage(damage);
        }
        DieOnImpact();
    }

    void DieFromRange()
    {
        KillBullet(dieFromRangePrtcls);
    }

    void DieOnImpact()
    {
        KillBullet(dieFromImpactPrtcls);
    }

    private void KillBullet(ParticleSystem[] particles = null)
    {
        foreach (var prtcl in particles)
        {
            prtcl.Play();
            prtcl.transform.parent = transform.parent;
        }
        foreach (var prtcl in runningParticle)
        {
            prtcl.enableEmission = false;
            prtcl.transform.parent = transform.parent;
            Destroy(prtcl.gameObject, .5f);
        }
        Destroy(this.gameObject);
    }
}
