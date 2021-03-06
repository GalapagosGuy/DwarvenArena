using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerSlots : MonoBehaviour
{
    [SerializeField] private GameObject rightHand = null;
    public GameObject RightHand { get => rightHand; }

    [SerializeField] private GameObject leftHand = null;
    public GameObject LeftHand { get => leftHand; }

    public LayerMask mouseLayer;
    public WeaponCustom equipedWeapon = null;
    public Spell equipedSpell = null;

    private Animator animator = null;

    private List<CastedSpell> hearingSpells = new List<CastedSpell>();

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

    public void UseSpell()
    {
        if (!equipedSpell)
            return;

        animator?.SetTrigger("spellTrigger");
        animator?.SetBool("usingSpell", true);
    }

    public void StopUsingSpell()
    {
        animator?.SetBool("usingSpell", false);

        CastedSpell[] spells = hearingSpells.ToArray();

        foreach (CastedSpell cs in spells)
        {
            cs.OnCastingSpellStop();
        }
    }

    public void ChangeWeapon(GameObject weapon)
    {
        if (equipedWeapon)
        {
            animator?.SetBool(equipedWeapon.AnimatorVariable, false);

            if (equipedWeapon.root)
                equipedWeapon.root.ReturnWeapon();
            else
                Destroy(equipedWeapon);
        }

        equipedWeapon = weapon.GetComponent<WeaponCustom>();

        equipedWeapon.transform.parent = rightHand.transform;
        equipedWeapon.transform.localPosition = Vector3.zero;
        equipedWeapon.transform.localRotation = Quaternion.identity;
        equipedWeapon.transform.localScale = Vector3.one;

        equipedWeapon.UpdateParentReference();
        animator?.SetBool(equipedWeapon.AnimatorVariable, true);
    }

    public void ChangeSpell(GameObject spell)
    {
        if (equipedSpell)
        {
            animator?.SetBool(equipedSpell.AnimatorVariable, false);

            if (equipedSpell.root)
                equipedSpell.root.ReturnSpell();
            else
                Destroy(equipedSpell);
        }

        equipedSpell = spell.GetComponent<Spell>();

        equipedSpell.transform.parent = leftHand.transform;
        equipedSpell.transform.localPosition = Vector3.zero;
        equipedSpell.transform.localRotation = Quaternion.identity;
        equipedSpell.transform.localScale = Vector3.one;

        equipedSpell.UpdateParentReference();
        animator?.SetBool(equipedSpell.AnimatorVariable, true);
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

    public void CastSpell()
    {
        Vector3 mousePosition = Vector3.zero;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit, mouseLayer))
        {
            equipedSpell?.Cast(this.transform.position, hit.point);
        }
    }

    public void AddSpellHearingForStopCasting(CastedSpell spell)
    {
        if (!hearingSpells.Contains(spell))
            hearingSpells.Add(spell);
    }

    public void RemoveSpellHearingForStopCasting(CastedSpell spell)
    {
        if (hearingSpells.Contains(spell))
            hearingSpells.Remove(spell);
    }

    #endregion
}
