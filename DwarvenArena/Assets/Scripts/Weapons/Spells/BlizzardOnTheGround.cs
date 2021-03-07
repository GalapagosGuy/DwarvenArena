using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardOnTheGround : CastedSpell
{
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;
    [SerializeField] private float damageOffset;

    [SerializeField] private float damageRadius = 3.0f;
    [SerializeField] private int bullets = 7;

    [SerializeField] private float chanceForPrison = 25.0f;
    public GameObject frozenPrison = null;

    public Animator animator;

    private List<IHitable> targetsHit = new List<IHitable>();

    public void DealDamage()
    {
        targetsHit.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, damageRadius);
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

                if (Random.Range(0, 100.0f) < chanceForPrison && !PrisonManager.Instance.CheckIfPrisonExistsForTarget(hitCollider.transform.root.gameObject))
                {
                    GameObject prison = Instantiate(frozenPrison, hitCollider.transform.position, Quaternion.identity);
                    prison.GetComponent<FrozenPrison>().CreatePrison(hitCollider.transform.root.gameObject);

                    PrisonManager.Instance.AddPrison(prison.GetComponent<FrozenPrison>());
                }

                /*if (fire)
                {
                    GameObject wildfire = Instantiate(fire, hitCollider.transform.root.transform.position, fire.transform.rotation, hitCollider.transform.root.transform);
                    Destroy(wildfire, 3.0f);

                    hitCollider.transform.root.GetComponent<Enemy>()?.BurnMaterial();
                }*/
            }
        }

        bullets--;

        if (bullets <= 0)
        {
            animator?.SetTrigger("destroyTrigger");
            animator.gameObject.transform.parent = null;
            Destroy(this.gameObject, 2f);
        }
    }

    public override void Initialize(Vector3 source, Vector3 target)
    {
        PlayerStats.Instance.SubstractMana(cost);
    }

    public override void OnCastingSpellStop()
    {

    }

    public float RandomDamage()
    {
        return Random.Range((int)(damage - damageOffset), (int)(damage + damageOffset) + 1);
    }

    public void DestroyByAnim()
    {
        //Destroy(this.gameObject);
    }
}
