using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mocks;

    private bool isTurnedOn;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn(bool isOn)
    {
        isTurnedOn = isOn;

        if(isTurnedOn)
        {
            GameObject mock = Instantiate(mocks[0]);

        }
    }
}
