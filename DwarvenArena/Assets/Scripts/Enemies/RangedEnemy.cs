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

    [SerializeField] protected Projectile projectilePrefab;

    public float attackCooldown;
    protected float cdCurrent;

    public float projectileSpeed;
    public float projectileDamage;
    public float projectileDamageOffset;
    public DamageType damageType;

    protected override void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyActions = EnemyActions.NOTHING;

        distancePrefs = new DistancePreferences(minPlayerDistance, maxPlayerDistance, minAllyDistance, maxAllyDistance);
        actionAvailability = new ActionAvailability();

        behaviourWeights = new ContextBasedSteeringBehaviourWeights();

        GetComponentInChildren<EnemyAnimEventsHandler>().OnStartAttack += CreateProjectile;
        //GetComponentInChildren<EnemyAnimEventsHandler>().OnEndAttack += ToggleAttackHitbox;

        hp = maxHp;
        UpdateUI(0);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////

    protected override void Act()
    {
        //find target
        if (target == null)
        {
            FindTarget();
            //if still null
            if (target == null)
            {
                enemyActions = EnemyActions.NOTHING;
                return;
            }
        }

        switch (enemyActions)
        {
            case EnemyActions.NOTHING:
                //check distance
                enemyActions = DistanceBasedDecision();
                break;
            case EnemyActions.FOLLOW:
                //move
                if (Vector3.Distance(transform.position, target.position) > distancePrefs.maxPrefferedDistanceFromPlayer)
                    Move(target);
                else
                    enemyActions = EnemyActions.ATTACK;
                break;
            case EnemyActions.ATTACK:
                //attack
                Attack();
                //check distance
                enemyActions = DistanceBasedDecision();
                break;
            case EnemyActions.RETREAT:
                //consider possible locations
                //move there
                if (Vector3.Distance(transform.position, target.position) < distancePrefs.maxPrefferedDistanceFromPlayer)
                    Move(target, true);
                else
                    enemyActions = EnemyActions.ATTACK;
                break;
        }
    }

    protected void WeightBasedDecision()
    {

    }

    protected float RandomDamage()
    {
        return Random.Range((int)(projectileDamage - projectileDamageOffset), (int)(projectileDamage + projectileDamageOffset) + 1);
    }

    protected void CreateProjectile(bool yes)
    {
        Projectile p = Instantiate<Projectile>(projectilePrefab, this.transform.position, Quaternion.identity, null);
        p.Initialize((target.position - this.transform.position).normalized, projectileSpeed, RandomDamage(), damageType);
    }
}
