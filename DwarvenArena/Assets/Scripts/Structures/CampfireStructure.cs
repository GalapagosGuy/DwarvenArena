using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireStructure : Structure
{
    [SerializeField]
    private float healingPower;
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(GameObject hero)
    {
        hero.GetComponent<PlayerStats>().HealUp(healingPower);
    }
}
