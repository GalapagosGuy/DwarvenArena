using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StructureContainer", menuName = "StructureContainer", order = 1)]
public class StructureContainer : ScriptableObject
{
    public GameObject structureObject;
    public GameObject structureMock;
    public int cost;
    public int tier;
    public Sprite structureSprite;
 
}
