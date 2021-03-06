using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMockWithMouse : MonoBehaviour
{
    public GameObject objectToMove;
    public LayerMask hitLayers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
        {
            objectToMove.transform.position = hit.point + new Vector3(0f, 0.5f, 0f);
        }

       // Vector3 temp = Input.mousePosition;
       // temp.z = 10f; // Set this to be the distance you want the object to be placed in front of the camera.
       // //objectToMove.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
       //objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, 0.5f, objectToMove.transform.position.z);
    }
}
