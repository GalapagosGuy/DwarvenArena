using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Structure : PlayerStuff, IHitable
{
    public int cost { get; private set; }
    public float hp { get; private set; }
    public float MaxHp { get => maxHp; set => maxHp = value; }

    [SerializeField]
    private float maxHp;

    [SerializeField]
    private GameObject hpObject;

    [SerializeField]
    private Image hpBar;

    private float timeToHeal = 1;
    private float repairAmount = 2;
    private float currentTime = 0;
    protected virtual void Start()
    {
        //hpObject.GetComponentInParent<Canvas>().gameObject.AddComponent<CanvasBillboard>();
        hpObject = GetComponentInChildren<hpObject>().gameObject;
        hpBar = GetComponentInChildren<hpBar>().transform.gameObject.GetComponent<Image>();
        hp = MaxHp;
        UpdateUI();
    }

    private void Update()
    {
        if(hp < MaxHp)
        {
            if (currentTime < timeToHeal)
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

    public void GetHit(float value, DamageType damageType)
    {
        if (damageType == DamageType.Blunt)
            value *= 1.5f;

        if (this == null)
            return;

        hp -= value;
        if (hp <= 0)
        {
            Debug.Log("Structure " + this.gameObject.name + " got destroyed");
            Destroy(this.gameObject);
        }
        UpdateUI();
            
    }

    public void Repair(float value)
    {
        hp += value;
        if (hp > MaxHp)
        {
            Debug.Log("Structure " + this.gameObject.name + " got fully repaired");
            hp = MaxHp;
        }
        UpdateUI();   

    }
    public abstract void Use(GameObject hero);

    public void UpdateUI()
    {
        hpBar.fillAmount = hp / MaxHp;
        if (hp == MaxHp)
            hpObject.SetActive(false);
        else
            hpObject.SetActive(true);

    }

}
