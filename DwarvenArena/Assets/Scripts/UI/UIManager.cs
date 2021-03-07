using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

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
    private TextMeshProUGUI waveText;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject skipWaitingWaveObject;

    [SerializeField]
    private GameObject waveTimerObject;

    [SerializeField]
    private TextMeshProUGUI waveTimerText;

    [Header("End game")]
    [SerializeField]
    private GameObject gameEndPanel;

    [SerializeField]
    private GameObject gameWonText;

    [SerializeField]
    private GameObject gameLostText;

    [SerializeField]
    private TextMeshProUGUI waveEndText;

    [SerializeField]
    private TextMeshProUGUI monsterKilledText;

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
        Time.timeScale = 1;
        UpdateUI();
        UpdateWaveText();
    }

    public void UpdateUI()
    {
        hpBar.fillAmount = playerStats.hp / playerStats.MaxHp;
        manaBar.fillAmount = playerStats.mana / playerStats.MaxMana;
        moneyText.text = playerStats.money.ToString();
    }

    public void UpdateWaveTimeText(float time)
    {
        waveTimerText.text = Mathf.RoundToInt(time).ToString();
    }

    public void UpdateWaveText()
    {
        waveText.text = (EnemySpawner.Instance.wave).ToString();
    }

    public void UpdateEndGameStatistics()
    {
        waveEndText.text = (EnemySpawner.Instance.wave -1 ).ToString();
        monsterKilledText.text = PlayerStats.Instance.monsterKilled.ToString();
    }

    public void GameWon()
    {
        gameEndPanel.SetActive(true);
        gameWonText.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        UpdateEndGameStatistics();

    }

    public void GameLost()
    {
        gameEndPanel.SetActive(true);
        gameLostText.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;

        UpdateEndGameStatistics();
    }

    public void ToggleSkip(bool toggle)
    {
        skipWaitingWaveObject.SetActive(toggle);
        waveTimerObject.SetActive(toggle);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleStructures(bool isOn)
    {
        structuresUIObject.SetActive(isOn);
    }

    public void ChangeIndicator(int category)
    {
        for (int i = 0; i < structuresUI.Length; i++)
        {
            structuresUI[i].GetComponent<StructureUI>().activeIndicator.SetActive(false);
            structuresUI[i].GetComponent<StructureUI>().tierText.text = "Tier " + (BuildManager.Instance.tierContainers[i].structureContainers[BuildManager.Instance.tiersToBuild[i]].tier + 1);
            structuresUI[i].GetComponent<StructureUI>().costText.text = BuildManager.Instance.tierContainers[i].structureContainers[BuildManager.Instance.tiersToBuild[i]].cost.ToString();
            structuresUI[i].GetComponent<StructureUI>().structureImage.sprite = BuildManager.Instance.tierContainers[i].structureContainers[BuildManager.Instance.tiersToBuild[i]].structureSprite;
        }

        structuresUI[category].GetComponent<StructureUI>().activeIndicator.SetActive(true);

    }

    public void GetHitEffect()
    {
        animator.SetTrigger("Blink");
    }


}
