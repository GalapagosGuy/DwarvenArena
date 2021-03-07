using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy
{
    public float minPlayerDistance;
    public float maxPlayerDistance;

    public float minAllyDistance;
    public float maxAllyDistance;

    [SerializeField] protected GameObject projectilePrefab;

    protected override void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        weaponCustom = GetComponentInChildren<WeaponCustom>();
        weaponCustom.Initialize(true);
        weaponCustom.DisableDealingDamage();
        enemyActions = EnemyActions.NOTHING;

        distancePrefs = new DistancePreferences(minPlayerDistance, maxPlayerDistance, minAllyDistance, maxAllyDistance);
        actionAvailability = new ActionAvailability();

        behaviourWeights = new ContextBasedSteeringBehaviourWeights();

        GetComponentInChildren<EnemyAnimEventsHandler>().OnStartAttack += ToggleAttackHitbox;
        //GetComponentInChildren<EnemyAnimEventsHandler>().OnEndAttack += ToggleAttackHitbox;

        hp = maxHp;
        UpdateUI(0);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    
    protected void CreateProjectile()
    {

    }
}
