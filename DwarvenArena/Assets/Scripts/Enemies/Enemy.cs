using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;

    protected Transform target = null;
    protected string allyTag = "Enemy";
    [Tooltip("Use for manual target setup")] public Transform debugSetTarget;

    public EnemyActions enemyActions { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////

    public class DistancePreferences
    {
        public float minPrefferedDistanceFromPlayer = 0;
        public float maxPrefferedDistanceFromPlayer = 1;

        public float minPrefferedDistanceFromAlly = 1;
        public float maxPrefferedDistanceFromAlly = Mathf.Infinity;
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

    ///////////////////////////////////////////////////////////////////////////////////////////
    
    public class ContextBasedSteeringBehaviourWeights
    {
        float[] weights;

        ContextBasedSteeringBehaviourWeights(int size)
        {
            weights = new float[size];
        }
    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyActions = EnemyActions.NOTHING;

        distancePrefs = new DistancePreferences();
        actionAvailability = new ActionAvailability();
    }

    // Update is called once per frame
    void Update()
    {
        if(actionAvailability.CheckIfFree())
            Act();

    }

    protected virtual void Act()
    {
        //debug
        if(debugSetTarget != null)
        {
            target = debugSetTarget;
            debugSetTarget = null;
        }

        //find target
        FindTarget();

        if (target == null)
            return;

        switch (enemyActions)
        {
            case EnemyActions.NOTHING:
                //check distance
                enemyActions = DistanceBasedDecision();
                break;
            case EnemyActions.FOLLOW:
                //move
                if (Vector3.Distance(transform.position, target.position) > distancePrefs.maxPrefferedDistanceFromPlayer)
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
                if (Vector3.Distance(transform.position, target.position) > distancePrefs.maxPrefferedDistanceFromPlayer)
                    MoveAwayFrom(target);
                else
                    enemyActions = EnemyActions.ATTACK;
                break;
        }
    }

    protected virtual EnemyActions DistanceBasedDecision()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > distancePrefs.maxPrefferedDistanceFromPlayer)
            return EnemyActions.FOLLOW;
        else if (distance < distancePrefs.minPrefferedDistanceFromPlayer)
            return EnemyActions.RETREAT;
        else
            return EnemyActions.ATTACK;
    }

    protected virtual void FindPlayer()
    {
        target = PlayerController.Instance.transform;
    }

    protected virtual void FindTarget()
    {
        if (target != null)
            return;

        Transform targetTransform = null;

        //find player singleton OR closest structure
        List<PlayerStuff> possibleTargets = FindObjectsOfType<PlayerStuff>().ToList();
        if (possibleTargets.Count == 0)
            Debug.Log("No PlayerStuff on Scene!");
        float minDistance = Mathf.Infinity;
        foreach(PlayerStuff target in possibleTargets)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetTransform = target.transform;
            }
        }
        target = targetTransform;
    }

    protected virtual void MoveTowards(Transform target)
    {
        actionAvailability.SetBusy();

        Collider[] allies = Physics.OverlapSphere(transform.position, distancePrefs.minPrefferedDistanceFromAlly);


        navMeshAgent.SetDestination(target.position);
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
