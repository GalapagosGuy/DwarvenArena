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

    private int currentCategory;
    private bool isTurnedOn;
    private PlayerController playerController;
    private GameObject mock;

    void Start()
    {
        BuildManager.Instance = this;

        playerController = GetComponent<PlayerController>();
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

            if (mock.GetComponent<Mock>().isColliding)
            {
                // Red
                structureMaterial.color = new Color(1f, 0, 0, 0.5f);
            }
            else
            {
                // white
                structureMaterial.color = new Color(1f, 1f, 1f, 0.5f);
                if(Input.GetMouseButtonDown(0))
                {
                    isTurnedOn = false;
                    GameObject buildedStructure = Instantiate(tierContainers[currentCategory].structureContainers[0].structureObject, mock.transform.position, mock.transform.rotation);
                    Destroy(mock);
                }
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

        if (currentCategory >= tierContainers.Length)
            currentCategory = 0;
        if (currentCategory < 0)
            currentCategory = tierContainers.Length - 1;

        if (gotChanged)
        {
            UIManager.Instance.ChangeIndicator(currentCategory);

            Destroy(mock);
            mock = Instantiate(tierContainers[currentCategory].structureContainers[0].structureMock);
        }

    }

    public void ToggleBuildingMode(bool isOn)
    {
        isTurnedOn = isOn;
        currentCategory = 0;

        if (isTurnedOn)
        {
            UIManager.Instance.ToggleStructures(isOn);
            UIManager.Instance.ChangeIndicator(currentCategory);
            mock = Instantiate(tierContainers[currentCategory].structureContainers[0].structureMock);
        }
        else
        {
            UIManager.Instance.ToggleStructures(isOn);
            Destroy(mock);

        }
    }
}
