using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerSlots))]
public class PlayerController : PlayerStuff
{
    public static PlayerController Instance = null;

    public LayerMask mouseLayer;
    public bool mouseLocked;

    private PlayerMovement playerMovement = null;
    private PlayerSlots playerSlots = null;
    public Vector3 mousePosition { get; private set; }
    private Detector detector;
    private BuildManager buildManager;

    private void Awake()
    {
        if (PlayerController.Instance == null)
            PlayerController.Instance = this;
        else
            Destroy(this);

        // To bylo nad Awake
        mousePosition = Vector3.zero;
        mouseLocked = false;
        
        playerMovement = GetComponent<PlayerMovement>();
        playerSlots = GetComponent<PlayerSlots>();
        detector = GetComponentInChildren<Detector>();
        buildManager = GetComponent<BuildManager>();
    }

    private void Update()
    {
        mousePosition = Vector3.zero;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, mouseLayer))
        {
            mousePosition = hit.point;
        }
        if(!mouseLocked)
            ProcessMouseInputs();
        ProcessKeyboardInputs();
    }

    private void ProcessMouseInputs()
    {
        if (Input.GetMouseButtonDown(0))
            playerSlots?.UseWeapon();
        else if (Input.GetMouseButtonDown(1))
            playerSlots?.UseSpell();

        if (!Input.GetMouseButton(1))
            playerSlots?.StopUsingSpell();
    }

    private void ProcessKeyboardInputs()
    {
        ProcessMovement();

        if (Input.GetKeyDown(InputMap.Action))
            detector.Use();

        if (Input.GetKeyDown(InputMap.Build))
            buildManager.TurnOn(true);

        if (Input.GetKeyUp(InputMap.Build))
            buildManager.TurnOn(false);
    }

    private void ProcessMovement()
    {
        bool isMoving = false;

        if (Input.GetKey(InputMap.MovementForward) || Input.GetKey(InputMap.MovementBackward) || Input.GetKey(InputMap.MovementLeft) || Input.GetKey(InputMap.MovementRight))
            isMoving = true;

        playerMovement?.Move(Input.GetAxis(InputMap.MovementHorizontal), Input.GetAxis(InputMap.MovementVertical), isMoving, mousePosition);

    }

}
