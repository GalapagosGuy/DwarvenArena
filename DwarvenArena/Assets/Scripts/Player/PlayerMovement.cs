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

    private float distFB;
    private Vector3 lastMousePosition = Vector3.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private int lastQuarter = 0;

    public Vector3 LastMousePosition { get => lastMousePosition; }

    public void Move(float movementLR, float movementFB, bool isMoving, Vector3 mousePosition)
    {
        Vector3 tmp = mousePosition;
        tmp.y = 0.0f;
        mousePosition = tmp;

        /*if (Vector3.Distance(mousePosition, this.transform.position) < 1.5f)
        {
            mousePosition = lastMousePosition;
        }*/

        lastMousePosition = mousePosition;

        this.transform.LookAt(mousePosition);

        float movementValue = Mathf.Abs(movementFB) > Mathf.Abs(movementLR) ? movementFB : movementLR;

        animator?.SetBool("isMoving", isMoving);

        Vector3 movement = Vector3.zero;

        movement += Vector3.right * movementLR;
        movement += Vector3.forward * movementFB;

        characterController?.Move(movement.normalized * movementSpeed * Time.deltaTime * Mathf.Abs(movementValue));

        Vector3 legsDirection = movement.normalized;

        int quarterIndex = (int)(this.transform.rotation.eulerAngles.y / 90);

        if (quarterIndex * 90 + 45 < this.transform.rotation.eulerAngles.y)
            quarterIndex++;

        quarterIndex = quarterIndex % 4;

        if (quarterIndex == 0 || quarterIndex == 2)
        {
            animator.SetFloat("movingFB", movementFB * (quarterIndex == 2 ? -1.0f : 1.0f));
            animator.SetFloat("movingLR", movementLR * (quarterIndex == 2 ? -1.0f : 1.0f));
        }
        else
        {
            animator.SetFloat("movingFB", movementLR * (quarterIndex == 3 ? -1.0f : 1.0f));
            animator.SetFloat("movingLR", movementFB * (quarterIndex == 3 ? 1.0f : -1.0f));
        }
    }
}
