using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMock : MonoBehaviour
{
    public WeaponCustom weaponReference;
    public MeshRenderer meshRenderer;
    public GameObject bloodSplash;

    private MeshCollider meshCollider;
    public bool enemyWeapon = false;
    public bool addedMana = false;

    public bool canDestroyStructures = true;

    [SerializeField] private float manaRestoreRatio = 0.05f;

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
        addedMana = false;
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
            iHitable.GetHit(weaponReference.RandomDamage(), weaponReference.DamageType);

            if (bloodSplash && !other.GetComponentInParent<Structure>())
            {
                GameObject bloodSplashGO = Instantiate(bloodSplash, other.transform.root.position + Vector3.up, transform.rotation);
                Destroy(bloodSplashGO, 2.0f);

                BloodManager.Instance.CreateBloodSplash(other.transform.position);
            }

            hitObjects.Add(iHitable);

            if (this.GetComponentInParent<PlayerStats>() && !addedMana)
            {
                PlayerStats.Instance.AddMana(weaponReference.Damage * manaRestoreRatio);
                addedMana = true;
            }
        }
    }
}
