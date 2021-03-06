using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStructure : Structure
{
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
        //return weapon to structure
        weaponCanBeTaken = true;
    }
}
