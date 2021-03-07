using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonManager : MonoBehaviour
{
    public static PrisonManager Instance = null;

    private void Awake()
    {
        if (PrisonManager.Instance == null)
            PrisonManager.Instance = this;
        else
            Destroy(this.gameObject);
    }

    private List<FrozenPrison> prisons = new List<FrozenPrison>();

    public void AddPrison(FrozenPrison prison)
    {
        prisons.Add(prison);
    }

    public void RemovePrison(FrozenPrison prison)
    {
        prisons.Remove(prison);
    }

    public bool CheckIfPrisonExistsForTarget(GameObject target)
    {
        FrozenPrison[] p = prisons.ToArray();

        foreach (FrozenPrison prison in p)
        {
            if (target == null)
                return false;

            if (prison && prison.GetTarget() == target)
            {
                return true;
            }
        }

        return false;
    }

    public void DestroyPrison(GameObject target)
    {
        FrozenPrison[] p = prisons.ToArray();

        foreach (FrozenPrison prison in p)
        {
            if (prison.GetTarget() == target)
            {
                prison.DestroyPrison();
            }
        }
    }
}
