using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;

    protected Transform target = null;
    public EnemyActions enemyActions { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////

    public class DistancePreferences
    {
        public float minPrefferedDistance = 0;
        public float maxPrefferedDistance = Mathf.Infinity;
    }
    public DistancePreferences distancePrefs { get; protected set; }

    ///////////////////////////////////////////////////////////////////////////////////////////

    public class ActionAvailability
    {
        public bool isFree = true;
        public float timeToSetFree = 0;

        public void SetBusy()
        {
            timeToSetFree = 1.0f;
            isFree = false;
        }

        public void SetBusy(float duration)
        {
            timeToSetFree = duration;
            isFree = false;
        }

        public bool CheckIfFree()
        {
            if (timeToSetFree > 0)
                timeToSetFree -= Time.deltaTime;
            else
                isFree = true;

            return isFree;
        }
    }
    public ActionAvailability actionAvailability { get; private set; }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyActions = EnemyActions.NOTHING;
    }

    // Update is called once per frame
    void Update()
    {
        if(actionAvailability.CheckIfFree())
            Act();
    }

    protected virtual void Act()
    {
        switch (enemyActions)
        {
            case EnemyActions.NOTHING:
                //find target
                FindTarget();
                //check distance
                enemyActions = DistanceBasedDecision();
                break;
            case EnemyActions.FOLLOW:
                //move
                if (Vector3.Distance(transform.position, target.position) > distancePrefs.maxPrefferedDistance)
                    MoveTowards(target);
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
                break;
        }
    }

    protected virtual EnemyActions DistanceBasedDecision()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > distancePrefs.maxPrefferedDistance)
            return EnemyActions.FOLLOW;
        else if (distance < distancePrefs.minPrefferedDistance)
            return EnemyActions.RETREAT;
        else
            return EnemyActions.ATTACK;
    }

    protected virtual void FindTarget()
    {
        if (target != null)
            return;

        Transform transform = null;

        //find player singleton OR closest structure

        target = transform;
    }

    protected virtual void MoveTowards(Transform target)
    {
        actionAvailability.SetBusy();
    }

    protected virtual void MoveAwayFrom(Transform target)
    {
        actionAvailability.SetBusy();
    }

    protected virtual void Attack()
    {
        actionAvailability.SetBusy();
    }
}
