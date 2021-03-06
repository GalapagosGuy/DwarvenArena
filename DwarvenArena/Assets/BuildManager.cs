using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] structures;

    [SerializeField]
    private GameObject[] mocks;


    [SerializeField]
    private LayerMask hitLayers;

    private int currentMock;
    private bool isTurnedOn;
    private PlayerController playerController;
    private GameObject mock;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
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
                GameObject buildedStructure = Instantiate(structures[currentMock], mock.transform.position, mock.transform.rotation);
                Destroy(mock);
 
            }
        }
    }

    public void ScrollingMocks()
    {
        bool gotChanged = false;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentMock++;
            gotChanged = true;
        }
            
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            gotChanged = true;
            currentMock--;
        }
           

        if (currentMock >= mocks.Length)
            currentMock = 0;
        if (currentMock < 0)
            currentMock = mocks.Length -1;

        if(gotChanged)
        {
            Destroy(mock);
            mock = Instantiate(mocks[currentMock]);
        }
        
    }

    public void TurnOn(bool isOn)
    {
        isTurnedOn = isOn;
        currentMock = 0;
        if (isTurnedOn)
        {
            mock = Instantiate(mocks[currentMock]);
        }
        else
            Destroy(mock);
    }
}
