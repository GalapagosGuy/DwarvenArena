using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFractured : MonoBehaviour
{
    private void Start()
    {
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in mrs)
        {
            mr.material.SetFloat("Display", -1.0f);
        }
    }
}
