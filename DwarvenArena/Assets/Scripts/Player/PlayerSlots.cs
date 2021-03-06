using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerSlots : MonoBehaviour
{
    [SerializeField] private GameObject rightHand = null;
    public GameObject RightHand { get => rightHand; }

    public WeaponCustom equipedWeapon = null;
    //public Spell equipedSpell = null;

    private Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UseWeapon()
    {
        if (!equipedWeapon)
            return;

        animator?.SetTrigger("attackTrigger");
    }

    public void ChangeWeapon(GameObject weapon)
    {
        if (equipedWeapon)
        {
            animator?.SetBool(equipedWeapon.AnimatorVariable, false);
            equipedWeapon.root.ReturnWeapon();
        }

        equipedWeapon = weapon.GetComponent<WeaponCustom>();

        equipedWeapon.transform.parent = rightHand.transform;
        equipedWeapon.transform.localPosition = Vector3.zero;
        equipedWeapon.transform.localRotation = Quaternion.identity;
        equipedWeapon.transform.localScale = Vector3.one;

        equipedWeapon.UpdateParentReference();
        animator?.SetBool(equipedWeapon.AnimatorVariable, true);
    }

    #region Animation Events

    public void EnableDealingMeleeDamage()
    {
        equipedWeapon?.EnableDealingDamage();
    }

    public void DisableDealingMeleeDamage()
    {
        equipedWeapon?.DisableDealingDamage();
    }

    #endregion
}
