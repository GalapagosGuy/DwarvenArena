using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerSlots))]
public class PlayerController : PlayerStuff
{
    public static PlayerController Instance = null;

    public LayerMask mouseLayer;

    private PlayerMovement playerMovement = null;
    private PlayerSlots playerSlots = null;
    public Vector3 mousePosition { get; private set; }
    private Detector detector;

    private void Awake()
    {
        if (PlayerController.Instance == null)
            PlayerController.Instance = this;
        else
            Destroy(this);

        // To bylo nad Awake
        mousePosition = Vector3.zero;

        playerMovement = GetComponent<PlayerMovement>();
        playerSlots = GetComponent<PlayerSlots>();
        detector = GetComponentInChildren<Detector>();
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

    }

    private void ProcessMovement()
    {
        bool isMoving = false;

        if (Input.GetKey(InputMap.MovementForward) || Input.GetKey(InputMap.MovementBackward) || Input.GetKey(InputMap.MovementLeft) || Input.GetKey(InputMap.MovementRight))
            isMoving = true;

        playerMovement?.Move(Input.GetAxis(InputMap.MovementHorizontal), Input.GetAxis(InputMap.MovementVertical), isMoving, mousePosition);

    }

}
