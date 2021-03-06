using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMock : MonoBehaviour
{
    public WeaponCustom weaponReference;

    private MeshCollider meshCollider;
    public bool enemyWeapon = false;

    private void Awake()
    {
        meshCollider = GetComponentInChildren<MeshCollider>();
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

        if (enemyWeapon && other.CompareTag("Enemy"))
            return;

        if(other is IHitable iHitable)
        {
            if (iHitable != null && !hitObjects.Contains(iHitable))
            {
                iHitable.GetHit(weaponReference.Damage, weaponReference.DamageType);
                hitObjects.Add(iHitable);
            }
        }
    }
}
