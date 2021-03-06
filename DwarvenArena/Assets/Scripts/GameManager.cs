using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance = this;
        playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubstractMoney(int value)
    {
        playerStats.SubstractMoney(value);
        UIManager.Instance.UpdateUI();

    }

    public void SubstractMana(float value)
    {
        playerStats.SubstractMana(value);
        UIManager.Instance.UpdateUI();
    }
}
