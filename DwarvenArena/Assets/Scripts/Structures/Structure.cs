using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Structure : PlayerStuff, IHitable
{
    public int cost { get; private set; }
    public float hp { get; private set; }
    
    [SerializeField]
    private float maxHp;

    [SerializeField]
    private GameObject hpObject;

    [SerializeField]
    private Image hpBar;

    protected virtual void Start()
    {
        //hpObject.GetComponentInParent<Canvas>().gameObject.AddComponent<CanvasBillboard>();
        hpObject = GetComponentInChildren<hpObject>().gameObject;
        hpBar = FindObjectOfType<Image>();
        hp = maxHp;
        UpdateUI();
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
        if (hp > maxHp)
        {
            Debug.Log("Structure " + this.gameObject.name + " got fully repaired");
            hp = maxHp;
        }
        UpdateUI();   

    }
    public abstract void Use(GameObject hero);

    public void UpdateUI()
    {
        hpBar.fillAmount = hp / maxHp;
        if (hp == maxHp)
            hpObject.SetActive(false);
        else
            hpObject.SetActive(true);

    }

}
