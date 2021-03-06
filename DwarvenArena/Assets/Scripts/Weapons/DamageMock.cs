using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMock : MonoBehaviour
{
    public WeaponCustom weaponReference;
    public MeshRenderer meshRenderer;

    private MeshCollider meshCollider;
    public bool enemyWeapon = false;

    public bool canDestroyStructures = true;

    private void Awake()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void EnableDealingDamage()
    {
        if (meshCollider != null)
            meshCollider.enabled = true;

        hitObjects.Clear();
    }

    public void DisableDealingDamage()
    {
        if (meshCollider != null)
            meshCollider.enabled = false;
    }

    private List<IHitable> hitObjects = new List<IHitable>();

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.gameObject == this.transform.root.gameObject)
            return;

        if (other.GetComponentInParent<Structure>() && !canDestroyStructures)
            return;

        if (enemyWeapon && other.transform.root.CompareTag("Enemy"))
            return;

        IHitable iHitable = other.GetComponentInParent<IHitable>();

        if (iHitable != null && !hitObjects.Contains(iHitable))
        {
            iHitable.GetHit(weaponReference.Damage, weaponReference.DamageType);
            hitObjects.Add(iHitable);
        }
    }
}
