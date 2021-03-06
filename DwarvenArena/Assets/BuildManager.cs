using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance = null;

    [SerializeField]
    public TierContainer[] tierContainers;

    [SerializeField]
    private LayerMask hitLayers;
    public Material structureMaterial;

    private GameObject[,] buildedStructures;

    public int[] tiersToBuild;

    private int currentCategory;
    private bool isTurnedOn;
    private PlayerController playerController;
    private GameObject mock;

    // DO ZMIANY <<<<<<<<<<<<<<<<<<<<<
    private int numberOfTiers = 2;

    void Start()
    {
        BuildManager.Instance = this;

        playerController = GetComponent<PlayerController>();

        buildedStructures = new GameObject[tierContainers.Length, numberOfTiers];
        tiersToBuild = new int[tierContainers.Length];
    }

    // Update is called once per frame
    void Update()
    {
        BuildingModeUpdate();
    }

    public void BuildingModeUpdate()
    {
        if (isTurnedOn)
        {
            ScrollingMocks();
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
            {
                mock.transform.position = hit.point + new Vector3(0f, 0.5f, 0f);
            }

            if (!mock.GetComponent<Mock>().isColliding)
            {
                structureMaterial.color = new Color(1f, 1f, 1f, 0.5f);  // WHITE COLOR, CAN BUILD
                if (PlayerStats.Instance.HasEnoughMoney(tierContainers[currentCategory].structureContainers[tiersToBuild[currentCategory]].cost) && Input.GetMouseButtonDown(0))
                {
                    // TAKE MONEY FROM DWARF'S POCKET
                    PlayerStats.Instance.SubstractMoney(tierContainers[currentCategory].structureContainers[tiersToBuild[currentCategory]].cost);

                    GameObject buildedStructure = Instantiate(tierContainers[currentCategory].structureContainers[tiersToBuild[currentCategory]].structureObject, mock.transform.position, mock.transform.rotation);
                    buildedStructures[currentCategory, tiersToBuild[currentCategory]] = buildedStructure;
                    UpdateTiersToBuild();
                    UIManager.Instance.ChangeIndicator(currentCategory);
                    Destroy(mock);
                    mock = Instantiate(tierContainers[currentCategory].structureContainers[tiersToBuild[currentCategory]].structureMock);

                    
                }
            }
            else
            {
                structureMaterial.color = new Color(1f, 0, 0, 0.5f); // RED COLOR, CANT BUILD
            }


        }
    }

    public void ScrollingMocks()
    {
        bool gotChanged = false;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentCategory--;
            gotChanged = true;
        }
            
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            gotChanged = true;
            currentCategory++;
        }

        if (currentCategory >= tierContainers.Length - 1)
            currentCategory = 0;
        if (currentCategory < 0)
            currentCategory = tierContainers.Length - 2;

        if (gotChanged)
        {
            UIManager.Instance.ChangeIndicator(currentCategory);
            UpdateTiersToBuild();
            Destroy(mock);
            mock = Instantiate(tierContainers[currentCategory].structureContainers[tiersToBuild[currentCategory]].structureMock);
        }

    }
    public int MissingTier(int category)
    {
        for (int i = 0; i < numberOfTiers; i++)
        {
            if (buildedStructures[category, i] == null)
                return i;
        }
        return 0;
    }
    public void ToggleBuildingMode(bool isOn)
    {
        isTurnedOn = isOn;
        currentCategory = 0;

        if (isTurnedOn)
        {
            UIManager.Instance.ToggleStructures(isOn);
            UIManager.Instance.ChangeIndicator(currentCategory);
            UpdateTiersToBuild();
            mock = Instantiate(tierContainers[currentCategory].structureContainers[tiersToBuild[currentCategory]].structureMock);
        }
        else
        {
            UIManager.Instance.ToggleStructures(isOn);
            Destroy(mock);

        }
    }

    public void UpdateTiersToBuild()
    {
        for (int i = 0; i < tiersToBuild.Length; i++)
        {
            tiersToBuild[i] = MissingTier(i);
        }
    }
}
