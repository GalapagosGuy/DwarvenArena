using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : PlayerStuff
{
    public static PlayerController Instance = null;

    public LayerMask mouseLayer;

    private PlayerMovement playerMovement = null;
    private Vector3 mousePosition = Vector3.zero;

    private void Awake()
    {
        if (PlayerController.Instance == null)
            PlayerController.Instance = this;
        else
            Destroy(this);

        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        mousePosition = Vector3.zero;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit, mouseLayer))
        {
            mousePosition = hit.point;
        }

        ProcessMouseInputs();
        ProcessKeyboardInputs();
    }

    private void ProcessMouseInputs()
    {
        if (Input.GetMouseButtonDown(0))
            GetComponent<Animator>()?.SetTrigger("attackTrigger");
    }

    private void ProcessKeyboardInputs()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        bool isMoving = false;

        if (Input.GetKey(InputMap.MovementForward) || Input.GetKey(InputMap.MovementBackward) || Input.GetKey(InputMap.MovementLeft) || Input.GetKey(InputMap.MovementRight))
            isMoving = true;

        playerMovement?.Move(Input.GetAxis(InputMap.MovementHorizontal), Input.GetAxis(InputMap.MovementVertical), isMoving, mousePosition);
    }
}
