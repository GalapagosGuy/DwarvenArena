using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private string animatorVariable = "";
    public GameObject castedSpell = null;

    public SpellStructure root { get; private set; } = null; // structure parent
    public string AnimatorVariable { get => animatorVariable; }
    private GameObject parent = null;

    private void Start()
    {
        root = GetComponentInParent<SpellStructure>();
        parent = this.transform.root.gameObject;
    }

    public void Cast(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (castedSpell)
        {
            GameObject go = Instantiate(castedSpell, this.transform.position, this.transform.rotation);

            go.GetComponent<CastedSpell>()?.Initialize(sourcePosition, targetPosition);
        }
    }

    public void UpdateParentReference()
    {
        parent = this.transform.root.gameObject;
    }
}
