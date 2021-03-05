using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters:")]
    [SerializeField] private float movementSpeed = 5.0f;

    [Header("References:")]
    public Animator animator = null;
    public CharacterController characterController = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    public void Move(float movementLR, float movementFB, bool isMoving)
    {
        bool movementValue = movementFB != 0 || movementLR != 0;

        animator?.SetBool("isMoving", isMoving);

        Vector3 movement = Vector3.zero;

        movement += Vector3.right * movementLR;
        movement += Vector3.forward * movementFB;

        characterController?.Move(movement * movementSpeed * Time.deltaTime);
    }
}
