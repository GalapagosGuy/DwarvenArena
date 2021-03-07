using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeerStructure : Structure
{
    [SerializeField]
    private float healingPower;

    [SerializeField]
    private float cooldownTime;

    [SerializeField]
    private Image cooldownBar;

    private float currentTime;
    private bool isReady;

    public GameObject beer;
    private float currentTime2 = 0;
    private float timeToRepair = 1;
    private float repairAmount = 2;
    protected override void Start()
    {
        base.Start();
        currentTime = 0;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= cooldownTime)
            {
                isReady = true;
                beer.SetActive(true);

            }
            UpdateUI();
        }

        if (hp < MaxHp)
        {
            if (currentTime < timeToRepair)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                Repair(repairAmount);
                currentTime = 0;
            }
        }
    }

    public void UpdateUI()
    {
        cooldownBar.fillAmount = currentTime / cooldownTime;
    }

    public override void Use(GameObject hero)
    {
        if (isReady)
        {
            hero.GetComponentInParent<PlayerStats>().AddMana(healingPower);
            isReady = false;
            currentTime = 0;
            beer.SetActive(false);
        }

    }
}
