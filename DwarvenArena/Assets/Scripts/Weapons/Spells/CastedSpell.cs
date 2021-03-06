using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastedSpell : MonoBehaviour
{
    public abstract void Initialize(Vector3 source, Vector3 target);
    public float cost;
}
