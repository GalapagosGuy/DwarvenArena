using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorStanding : Structure
{
    public enum ArmorTier
    {
        first,
        second,
        third
    }

    [SerializeField]
    private ArmorTier armorTier;

    public override void Use(GameObject hero)
    {
        
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (armorTier == ArmorTier.first)
            PlayerStats.Instance.armor.ActivateTierOne();
        if (armorTier == ArmorTier.second)
            PlayerStats.Instance.armor.ActivateTierTwo();
        if (armorTier == ArmorTier.third)
            PlayerStats.Instance.armor.ActivateTierThree();
    }

}
