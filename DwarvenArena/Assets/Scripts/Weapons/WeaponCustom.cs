using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WeaponCustom : MonoBehaviour
{
    [SerializeField] private DamageType damageType;
    [SerializeField] private float damage = 0.0f;
    [SerializeField] private string animatorVariable = "";

    public WeaponStructure root { get; private set; } = null; // structure parent
    public string AnimatorVariable { get => animatorVariable; }

    private GameObject parent = null;
    private BoxCollider boxCollider = null;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        root = GetComponentInParent<WeaponStructure>();
        parent = this.transform.root.gameObject;
    }

    private List<IHitable> hitObjects = new List<IHitable>();

    public void EnableDealingDamage()
    {
        if (boxCollider)
            boxCollider.enabled = true;

        hitObjects.Clear();
    }

    public void DisableDealingDamage()
    {
        if (boxCollider)
            boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == parent)
            return;

        IHitable iHitable = other.GetComponentInParent<IHitable>();

        if (iHitable != null && !hitObjects.Contains(iHitable))
        {
            iHitable.GetHit(damage);
            hitObjects.Add(iHitable);
        }
    }

    public void UpdateParentReference()
    {
        parent = this.transform.root.gameObject;
    }
}

