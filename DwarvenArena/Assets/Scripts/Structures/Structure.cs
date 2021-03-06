using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : MonoBehaviour, IHitable
{
    public int cost { get; private set; }
    public float hp { get; private set; }
    [SerializeField]
    private float maxHp;

    protected virtual void Start()
    {
        hp = maxHp;
    }

    public void GetHit(float value)
    {
        hp -= value;
        if (hp <= 0)
        {
            Debug.Log("Structure " + this.gameObject.name + " got destroyed");
            Destroy(this.gameObject);
        }
            
    }

    public void Repair(float value)
    {
        hp += value;
        if (hp > maxHp)
        {
            Debug.Log("Structure " + this.gameObject.name + " got fully repaired");
            hp = maxHp;
        }
            

    }
    public abstract void Use(GameObject hero);

}
