using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : CastedSpell
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float damage = 50.0f;
    [SerializeField] private float explosionRange = 2.0f;
    [SerializeField] private DamageType damageType;

    public GameObject explosionParticles = null;

    private Vector3 targetPosition;

    public override void Initialize(Vector3 source, Vector3 target)
    {
        GameManager.Instance.SubstractMana(cost);
        targetPosition = target;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

        if (transform.position == targetPosition)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject != PlayerController.Instance.gameObject)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, explosionRange);
        foreach (var hitCollider in hitColliders)
        {
            IHitable iHitable = hitCollider.GetComponentInParent<IHitable>();
            IHitable playerHitable = PlayerController.Instance.gameObject.GetComponent<PlayerStats>();

            if (iHitable != null && playerHitable != iHitable)
            {
                iHitable.GetHit(damage, damageType);
            }
        }

        GameObject explosion = Instantiate(explosionParticles, this.transform.position, this.transform.rotation);
        Destroy(explosion, 2.0f);
    }

    public override void OnCastingSpellStop()
    {

    }
}
