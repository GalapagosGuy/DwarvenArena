using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStructure : Structure
{
    [SerializeField]
    private Weapon.WeaponType weaponType;

    protected override void Start()
    {
        base.Start();

    }

    public override void Use(GameObject hero)
    {
        hero.GetComponent<Weapon>().ChangeWeapon(weaponType);
    }
}
