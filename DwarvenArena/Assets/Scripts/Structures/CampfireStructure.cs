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

    private float currentTime;
    private bool isReady;

    protected override void Start()
    {
        base.Start();
        currentTime = 0;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isReady)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= cooldownTime)
            {
                isReady = true;
                
            }
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        cooldownBar.fillAmount = currentTime / cooldownTime;
    }

    public override void Use(GameObject hero)
    {
        if(isReady)
        {
            Debug.Log("Player got healed from " + this.gameObject.name);
            hero.GetComponentInParent<PlayerStats>().HealUp(healingPower);
            isReady = false;
            currentTime = 0;
        }
       
    }
}
