using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageFromAudiance : Structure
{
    [SerializeField]
    private int moneyAward;

    public GameObject soundObject;

    public override void Use(GameObject hero)
    {
        GameObject go = Instantiate(soundObject, this.transform.position, this.transform.rotation);
        Destroy(go, 2.0f);

        hero.GetComponent<PlayerStats>().AddMoney(moneyAward);
        hero.GetComponentInChildren<Detector>().DeleteItem();
        Destroy(this.gameObject);
    }
}
