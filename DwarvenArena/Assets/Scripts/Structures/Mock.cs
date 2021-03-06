using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mock : MonoBehaviour
{
   public bool isColliding { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            isColliding = false;
    }
}
