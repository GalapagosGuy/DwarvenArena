using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters:")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 5.0f;

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
        float movementValue = Mathf.Abs(movementFB) > Mathf.Abs(movementLR) ? movementFB : movementLR;

        animator?.SetBool("isMoving", isMoving);

        Vector3 movement = Vector3.zero;

        movement += Vector3.right * movementLR;
        movement += Vector3.forward * movementFB;

        if (isMoving)
            RotatePlayer(movementLR, movementFB);

        characterController?.Move(movement.normalized * movementSpeed * Time.deltaTime * Mathf.Abs(movementValue));
    }

    private void RotatePlayer(float movementLR, float movementFB)
    {
        float desirableRotation = GetDesirableRotation(movementLR, movementFB);

        Vector3 desirableRotationVector = new Vector3(0, desirableRotation, 0);

        Quaternion angle = Quaternion.Lerp(this.transform.rotation, Quaternion.Normalize(Quaternion.Euler(desirableRotationVector.x, desirableRotationVector.y, desirableRotationVector.z)), Time.deltaTime * rotationSpeed);

        this.transform.rotation = angle;
    }

    private float GetDesirableRotation(float movementLR, float movementFB)
    {
        float desirableRotation = 0.0f;

        if (movementFB > 0.0f)
        {
            if (movementLR > 0.0f)
            {
                desirableRotation = 45.0f;
            }
            else if (movementLR < -0.0f)
            {
                desirableRotation = 315.0f;
            }
            else
            {
                desirableRotation = 0.0f;
            }
        }
        else if (movementFB < -0.0f)
        {
            if (movementLR > 0.7f)
            {
                desirableRotation = 135.0f;
            }
            else if (movementLR < -0.0f)
            {
                desirableRotation = 225.0f;
            }
            else
            {
                desirableRotation = 180.0f;
            }
        }
        else if (movementLR > 0.0f)
            desirableRotation = 90.0f;
        else if (movementLR < -0.0f)
            desirableRotation = 270.0f;

        return desirableRotation;
    }
}
