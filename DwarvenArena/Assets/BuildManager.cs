using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance = null;

    [SerializeField]
    private TierContainer[] tierContainers;

    [SerializeField]
    private LayerMask hitLayers;

    private int currentStructure;
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
        if (isTurnedOn)
        {
            ScrollingMocks();

        }


        /*
        if(isTurnedOn)
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
            {
                mock.transform.position = hit.point + new Vector3(0f, 0.5f, 0f);
            }

            ScrollingMocks();

            if (!mock.GetComponent<Mock>().isColliding && Input.GetMouseButtonDown(0))
            {
                isTurnedOn = false;
               // GameObject buildedStructure = Instantiate(structures[currentMock], mock.transform.position, mock.transform.rotation);
                Destroy(mock);
 
            }
        }
        */
    }

    public void ScrollingMocks()
    {
        bool gotChanged = false;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentStructure--;
            gotChanged = true;
        }
            
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            gotChanged = true;
            currentStructure++;
        }

        if (currentStructure >= tierContainers.Length)
            currentStructure = 0;
        if (currentStructure < 0)
            currentStructure = tierContainers.Length - 1;

        if (gotChanged)
        {
            UIManager.Instance.ChangeIndicator(currentStructure);

            Destroy(mock);
           // mock = Instantiate(mocks[currentMock]);
        }

    }

    public void ToggleBuildingMode(bool isOn)
    {
        isTurnedOn = isOn;
        currentStructure = 0;

        if (isTurnedOn)
        {
            UIManager.Instance.ToggleStructures(isOn);
            UIManager.Instance.ChangeIndicator(currentStructure);
            //mock = Instantiate(mocks[currentMock]);
        }
        else
        {
            UIManager.Instance.ToggleStructures(isOn);
            Destroy(mock);

        }
    }
}
