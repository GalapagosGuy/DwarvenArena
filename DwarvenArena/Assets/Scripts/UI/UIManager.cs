using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("Structures UI")]
    public GameObject[] structuresUI;

    public GameObject structuresUIObject;

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

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleStructures(bool isOn)
    {
        structuresUIObject.SetActive(isOn);
    }

    public void ChangeIndicator(int structure)
    {
        for (int i = 0; i < structuresUI.Length; i++)
        {
            structuresUI[i].GetComponent<StructureUI>().activeIndicator.SetActive(false);
        }

        structuresUI[structure].GetComponent<StructureUI>().activeIndicator.SetActive(true);
    }


}
