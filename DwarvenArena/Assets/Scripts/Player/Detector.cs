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

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponentInParent<Structure>())
        {

            interactiveInRange = true;
            detectedStructure = other.GetComponentInParent<Structure>();
            if (other.GetComponent<Outline>())
                other.GetComponent<Outline>().enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                detectedStructure.Use(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Structure>())
        {
            interactiveInRange = false;
            detectedStructure = null;

            if (other.GetComponent<Outline>())
                other.GetComponent<Outline>().enabled = false;
        }
    }
}
