using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimEventsHandler : MonoBehaviour
{
    public Action<bool> OnStartAttack;
    public Action<bool> OnEndAttack;

    public void EnableDealingMeleeDamage()
    {
        OnStartAttack?.Invoke(true);
    }

    public void DisableDealingMeleeDamage()
    {
        OnEndAttack?.Invoke(false);
    }
}
