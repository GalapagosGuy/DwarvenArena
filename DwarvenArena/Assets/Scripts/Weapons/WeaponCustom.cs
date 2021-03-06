using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WeaponCustom : MonoBehaviour
{
    [SerializeField] private DamageType damageType;
    [SerializeField] private float damage = 0.0f;
    [SerializeField] private string animatorVariable = "";

    public GameObject mock;
    public WeaponStructure root { get; private set; } = null; // structure parent
    public string AnimatorVariable { get => animatorVariable; }
    public float Damage { get => damage; }
    public DamageType DamageType { get => damageType; }

    private GameObject parent = null;
    private BoxCollider boxCollider = null;
    private DamageMock createdMock = null;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        root = GetComponentInParent<WeaponStructure>();
        parent = this.transform.root.gameObject;
    }

    private List<IHitable> hitObjects = new List<IHitable>();

    public void Initialize(bool enemyWeapon = false)
    {
        GameObject mockGO = Instantiate(mock, this.transform.root.transform);
        createdMock = mockGO.GetComponent<DamageMock>();
        createdMock.weaponReference = this;
        createdMock.enemyWeapon = enemyWeapon;
    }

    public void OnWeaponReturn()
    {
        if (createdMock)
            Destroy(createdMock.gameObject);
    }

    public void EnableDealingDamage()
    {
        createdMock.EnableDealingDamage();
        /*if (boxCollider)
            boxCollider.enabled = true;

        hitObjects.Clear();*/
    }

    public void DisableDealingDamage()
    {
        createdMock.DisableDealingDamage();
        //if (boxCollider)
        //    boxCollider.enabled = false;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == parent)
            return;

        IHitable iHitable = other.GetComponentInParent<IHitable>();

        if (iHitable != null && !hitObjects.Contains(iHitable))
        {
            iHitable.GetHit(damage, damageType);
            hitObjects.Add(iHitable);
        }
    }*/

    public void UpdateParentReference()
    {
        parent = this.transform.root.gameObject;
    }
}

