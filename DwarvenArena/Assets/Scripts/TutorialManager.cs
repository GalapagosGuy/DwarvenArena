using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public List<GameObject> tutorialPopups;
    public int nextPopupIndex = 0;

    private EnemySpawner spawnerRef;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        spawnerRef = EnemySpawner.Instance;
        SpawnTextWindow();
    }

    public void SpawnTextWindow()
    {
        tutorialPopups[nextPopupIndex].SetActive(true);
        nextPopupIndex++;
    }

    public void ContinueTutorial()
    {
        tutorialPopups[nextPopupIndex - 1].SetActive(false);
    }
}
