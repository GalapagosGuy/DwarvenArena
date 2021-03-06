using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageFromAudiance : Structure
{
    [SerializeField]
    private int moneyAward;

    public override void Use(GameObject hero)
    {
        hero.GetComponentInParent<PlayerStats>().AddMoney(moneyAward);
        hero.GetComponent<Detector>().DeleteItem();
        Destroy(this.gameObject);
    }
}
