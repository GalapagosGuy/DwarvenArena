using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance = null;

    public LayerMask mouseLayer;

    private void Awake()
    {
        if (MouseController.Instance == null)
            MouseController.Instance = this;
        else
            Destroy(this.gameObject);
    }

    public GameObject mousePointer = null;

    private void Start()
    {
        //Cursor.visible = false;
    }

    private void Update()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit, Mathf.Infinity, mouseLayer))
        {
            mousePointer.transform.position = hit.point + new Vector3(0.0f, 0.05f, 0.0f);
        }
    }
}
