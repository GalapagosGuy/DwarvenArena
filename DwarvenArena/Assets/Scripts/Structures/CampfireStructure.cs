using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampfireStructure : Structure
{
    [SerializeField]
    private float healingPower;

    [SerializeField]
    private float cooldownTime;

    [SerializeField]
    private Image cooldownBar;


    private bool isReady;

    public GameObject mead;

    private float currentHealTime = 0;
    protected override void Start()
    {
        base.Start();
        currentHealTime = 0;
        currentRepairTime = 0;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // REPAIRING
        if (hp < MaxHp)
        {
            if (currentRepairTime < timeToRepair)
            {
                currentRepairTime += Time.deltaTime;
            }
            else
            {
                Repair(repairAmount);
                currentRepairTime = 0;
            }
        }

        // SPAWNING FOOD
        if (!isReady)
        {
            currentHealTime += Time.deltaTime;
            if (currentHealTime >= cooldownTime)
            {
                isReady = true;
                mead.SetActive(true);

            }
            UpdateUI();
        }
        
        
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        cooldownBar.fillAmount = currentHealTime / cooldownTime;
        
    }

    public override void Use(GameObject hero)
    {
        if (isReady)
        {
            Debug.Log("Player got healed from " + this.gameObject.name);
            hero.GetComponentInParent<PlayerStats>().PercentHealUp(healingPower);
            isReady = false;
            currentHealTime = 0;
            mead.SetActive(false);
            GetComponent<AudioSource>()?.Play();
        }

    }
}
