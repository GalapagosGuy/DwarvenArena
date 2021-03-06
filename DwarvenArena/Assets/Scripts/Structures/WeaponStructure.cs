using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStructure : Structure
{
    public GameObject holder = null;
    public GameObject weaponReference = null;

    private bool weaponCanBeTaken = true;

    protected override void Start()
    {
        base.Start();

    }

    public override void Use(GameObject hero)
    {
        if (weaponCanBeTaken)
        {
            PlayerSlots playerSlots = hero.GetComponent<PlayerSlots>();

            if (playerSlots)
            {
                playerSlots.ChangeWeapon(weaponReference);
                weaponCanBeTaken = false;
            }
        }
    }

    public void ReturnWeapon()
    {
        if (holder)
            weaponReference.transform.parent = holder.transform;
        weaponReference.transform.localPosition = Vector3.zero;
        weaponReference.transform.localRotation = Quaternion.identity;
        weaponReference.transform.localScale = Vector3.one;

        weaponCanBeTaken = true;
    }
}
