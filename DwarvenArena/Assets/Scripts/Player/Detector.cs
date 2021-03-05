using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private bool interactiveInRange;
    private Structure detectedStructure;

    public void Use()
    {
        detectedStructure.Use(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<Structure>())
        {
            interactiveInRange = true;
            detectedStructure = other.GetComponentInParent<Structure>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Structure>())
        {
            interactiveInRange = false;
            detectedStructure = null;
        }
    }
}
