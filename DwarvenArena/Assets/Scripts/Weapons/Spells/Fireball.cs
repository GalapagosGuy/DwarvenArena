using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : CastedSpell
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float damage = 50.0f;
    [SerializeField] private float damageOffset;
    [SerializeField] private float explosionRange = 2.0f;
    [SerializeField] private DamageType damageType;

    public GameObject fire;

    public GameObject explosionParticles = null;

    private Vector3 targetPosition;

    public float RandomDamage()
    {
        return Random.Range((int)(damage - damageOffset), (int)(damage + damageOffset) + 1);
    }

    public override void Initialize(Vector3 source, Vector3 target)
    {
        PlayerStats.Instance.SubstractMana(cost);
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

    private List<IHitable> targetsHit = new List<IHitable>();

    private void OnDestroy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, explosionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<Structure>())
                continue;

            IHitable iHitable = hitCollider.GetComponentInParent<IHitable>();
            IHitable playerHitable = PlayerController.Instance.gameObject.GetComponent<PlayerStats>();

            if (iHitable != null && playerHitable != iHitable && !targetsHit.Contains(iHitable))
            {
                targetsHit.Add(iHitable);
                iHitable.GetHit(RandomDamage(), damageType);

                if (fire)
                {
                    GameObject wildfire = Instantiate(fire, hitCollider.transform.root.transform.position, fire.transform.rotation, hitCollider.transform.root.transform);
                    Destroy(wildfire, 3.0f);

                    hitCollider.transform.root.GetComponent<Enemy>()?.BurnMaterial();
                }
            }
        }

        GameObject explosion = Instantiate(explosionParticles, this.transform.position, this.transform.rotation);
        Destroy(explosion, 2.0f);
    }

    public override void OnCastingSpellStop()
    {

    }
}
