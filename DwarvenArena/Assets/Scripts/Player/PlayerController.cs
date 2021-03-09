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

    private float startY;
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
        startY = transform.position.y;
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
        if (!mouseLocked)
        {
            if (Input.GetMouseButton(0))
            {
                playerSlots?.UseWeapon();
            }
        }


        if (Input.GetMouseButtonDown(1))
            playerSlots?.UseSpell();

        if (Input.GetMouseButtonUp(1))
            playerSlots?.StopUsingSpell();
    }

    private void ProcessKeyboardInputs()
    {
        ProcessMovement();

        if (Input.GetKeyDown(InputMap.Action))
            detector.Use();

        if (Input.GetKeyDown(InputMap.Build))
            BuildManager.Instance.ToggleBuildingMode();

        if (Input.GetKeyDown(InputMap.Skip))
        {
            UIManager.Instance.ToggleSkip(false);
            EnemySpawner.Instance.SkipWave();
        }
        if(!Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                UIManager.Instance.TogglePause();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F1))
                UIManager.Instance.TogglePause();
        }

    }

    private void ProcessMovement()
    {
        bool isMoving = false;

        if (Input.GetKey(InputMap.MovementForward) || Input.GetKey(InputMap.MovementBackward) || Input.GetKey(InputMap.MovementLeft) || Input.GetKey(InputMap.MovementRight))
            isMoving = true;

        playerMovement?.Move(Input.GetAxis(InputMap.MovementHorizontal), Input.GetAxis(InputMap.MovementVertical), isMoving, mousePosition);
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);

    }

}
