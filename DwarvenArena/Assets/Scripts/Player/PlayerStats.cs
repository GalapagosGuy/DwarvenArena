using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IHitable
{
    [SerializeField]
    private float maxHp;
    public float hp { get; private set; }
    public int money { get; private set; }
    public float MaxHp { get => maxHp; private set => maxHp = value; }

    void Start()
    {
        hp = maxHp * .25f;
        money = 0;
        UIManager.Instance?.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetHit(float value)
    {
        hp -= value;
        if (hp <= 0)
        {
            Debug.Log("PLAYER IS DEAD");
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
        UIManager.Instance.UpdateUI();

    }

    public bool HasEnoughMoney(int value)
    {
        return value <= money ? true : false;
    }
}
