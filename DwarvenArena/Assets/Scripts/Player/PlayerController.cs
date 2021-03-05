using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    private PlayerMovement playerMovement = null;

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
        ProcessMouseInputs();
        ProcessKeyboardInputs();
    }

    private void ProcessMouseInputs()
    {

    }

    private void ProcessKeyboardInputs()
    {
        bool isMoving = false;

        if (Input.GetKey(InputMap.MovementForward) || Input.GetKey(InputMap.MovementBackward) || Input.GetKey(InputMap.MovementLeft) || Input.GetKey(InputMap.MovementRight))
            isMoving = true;

        playerMovement?.Move(Input.GetAxis(InputMap.MovementHorizontal), Input.GetAxis(InputMap.MovementVertical), isMoving);
    }
}
