using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    [SerializeField]
    private Image hpBar;

    [SerializeField]
    private Image manaBar;

    [SerializeField]
    private TextMeshProUGUI moneyText;

    [SerializeField]
    private GameObject gameEndPanel;

    [SerializeField]
    private GameObject gameWonText;

    [SerializeField]
    private GameObject gameLostText;

    private PlayerStats playerStats;

    private void Awake()
    {
        UIManager.Instance = this;
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        hpBar.fillAmount = playerStats.hp / playerStats.MaxHp;
        manaBar.fillAmount = playerStats.mana / playerStats.MaxMana;
        moneyText.text = playerStats.money.ToString();
    }

    public void GameWon()
    {
        gameEndPanel.SetActive(true);
        gameWonText.SetActive(true);

    }

    public void GameLost()
    {
        gameEndPanel.SetActive(true);
        gameLostText.SetActive(true);
    }

}
