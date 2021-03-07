using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Vector3 direction;
    protected float speed;
    protected float damage;
    protected DamageType type;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 dir, float speed, float damage, DamageType type)
    {
        direction = dir;
        this.speed = speed;
        this.damage = damage;
        this.type = type;

        rb.AddForce(dir * speed);

        Destroy(this.gameObject, 8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Structure s = other.GetComponentInParent<Structure>();
        if(other.CompareTag("Player") || s != null)
        {
            IHitable hit = other.GetComponentInParent<IHitable>();
            hit.GetHit(damage, type);
            Destroy(this.gameObject);
        }
    }
}
