using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IHitable
{
    public static PlayerStats Instance = null;

    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float maxMana;
    [SerializeField]
    private float manaRegen;

    
    public float hp { get; private set; }
    public float mana { get; private set; }
    public int money { get; private set; }
    public float MaxHp { get => maxHp; private set => maxHp = value; }
    public float MaxMana { get => maxMana; private set => maxMana = value; }
    public int monsterKilled { get; private set; }

    public PlayerSlots playerSlots;

    void Start()
    {
        PlayerStats.Instance = this;
        playerSlots = GetComponent<PlayerSlots>();
        hp = maxHp;//* .25f;
        mana = maxMana;
        money = 500;
        UIManager.Instance?.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (mana < maxMana)
        {
            mana += manaRegen * Time.deltaTime;
            if (mana > maxMana)
                mana = maxMana;
            UIManager.Instance?.UpdateUI();
        }
    }

    public void GetHit(float value, DamageType damageType)
    {
        hp -= value;
        if (hp <= 0)
        {
            Debug.Log("PLAYER IS DEAD");
            UIManager.Instance.GameLost();
            Destroy(this.gameObject);
        }

        UIManager.Instance?.UpdateUI();
    }

    public void HealUp(float value)
    {
        hp += value;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        UIManager.Instance.UpdateUI();

    }

    public void AddMana(float value)
    {
        mana += value;
        if (mana > maxMana)
            mana = maxMana;
        UIManager.Instance?.UpdateUI();
    }

    public void AddMoney(int value)
    {
        money += value;
        UIManager.Instance?.UpdateUI();

    }

    public void SubstractMoney(int value)
    {
        money -= value;
        if (money < 0)
            money = 0;

    }

    public void SubstractMana(float value)
    {
        mana -= value;
        if (mana < 0)
            mana = 0;
    }

    public bool HasEnoughMoney(int value)
    {
        return value <= money ? true : false;
    }

    public bool HasEnoughMana(float value)
    {
        return value <= mana ? true : false;
    }

    public void AddKill()
    {
        monsterKilled++;
    }
}
