using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour, IHitable
{
    protected NavMeshAgent navMeshAgent;
    protected EnemyAnimator enemyAnimator;
    [SerializeField] protected TextMeshProUGUI damageTextPrefab;
    protected WeaponCustom weaponCustom;
    [SerializeField]
    private Image hpBar;
    [SerializeField]
    private GameObject hpObject;

    public DamageType weakness;

    protected Transform target = null;
    protected string allyTag = "Enemy";
    [Tooltip("Use for manual target setup")] public Transform debugSetTarget;

    public float maxHp = 100;
    protected float hp;

    public EnemyActions enemyActions { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////

    public class DistancePreferences
    {
        public float minPrefferedDistanceFromPlayer = 0f;
        public float maxPrefferedDistanceFromPlayer = 2.0f;

        public float minPrefferedDistanceFromAlly = 4.0f;
        public float maxPrefferedDistanceFromAlly = Mathf.Infinity;

        public DistancePreferences()
        { }

        public DistancePreferences(float minPlayer, float maxPlayer, float minAlly, float maxAlly)
        {
            minPrefferedDistanceFromPlayer = minPlayer;
            maxPrefferedDistanceFromPlayer = maxPlayer;
            minPrefferedDistanceFromAlly = minAlly;
            maxPrefferedDistanceFromAlly = maxAlly;
        }
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
    public ActionAvailability actionAvailability { get; protected set; }

    ///////////////////////////////////////////////////////////////////////////////////////////

    public class ContextBasedSteeringBehaviourWeights
    {
        public float[] weights;
        public static Vector3[] weightVectors;

        public ContextBasedSteeringBehaviourWeights()
        {
            weights = new float[8];
            weightVectors = new Vector3[8] {Vector3.forward, Vector3.RotateTowards(Vector3.forward, Vector3.right, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.right, Vector3.RotateTowards(Vector3.right, Vector3.back, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.back, Vector3.RotateTowards(Vector3.back, Vector3.left, 45.0f * Mathf.Deg2Rad, 0),
            Vector3.left, Vector3.RotateTowards(Vector3.left, Vector3.forward, 45.0f * Mathf.Deg2Rad, 0)};
        }
    }
    public ContextBasedSteeringBehaviourWeights behaviourWeights { get; protected set; }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        weaponCustom = GetComponentInChildren<WeaponCustom>();
        weaponCustom.Initialize(true);
        weaponCustom.DisableDealingDamage();
        enemyActions = EnemyActions.NOTHING;

        distancePrefs = new DistancePreferences();
        actionAvailability = new ActionAvailability();

        behaviourWeights = new ContextBasedSteeringBehaviourWeights();

        GetComponentInChildren<EnemyAnimEventsHandler>().OnStartAttack += ToggleAttackHitbox;
        GetComponentInChildren<EnemyAnimEventsHandler>().OnEndAttack += ToggleAttackHitbox;

        hp = maxHp;
        UpdateUI(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (actionAvailability.CheckIfFree())
            Act();

    }

    protected virtual void Act()
    {
        //debug
        if (debugSetTarget != null)
        {
            target = debugSetTarget;
            debugSetTarget = null;
        }

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
                if (Vector3.Distance(transform.position, target.position) > distancePrefs.maxPrefferedDistanceFromPlayer)
                    Move(target, true);
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
        float random = Random.Range(0.0f, 1.0f);
        if (random > .4f)
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

        float random = Random.Range(0.0f, 1.0f);
        float minDistance = Mathf.Infinity;
        foreach (PlayerStuff p in possibleTargets)
        {
            if (p is PlayerController player)
            {
                if (random < .9f)
                {
                    target = player.transform;
                    return;
                }
            }
            else
            {
                if (Vector3.Distance(p.transform.position, this.transform.position) < minDistance)
                {
                    targetTransform = p.transform;
                    minDistance = Vector3.Distance(p.transform.position, this.transform.position);
                }
            }
        }

        target = targetTransform;
    }

    //Weights start from pointing upwards and go clockwise
    protected virtual void CalculateContextWeights(Transform target, Collider[] surrounding, float enemySeparationBias = .3f, float playerBias = 2.0f, bool towardsTarget = true)
    {
        //copy original vectors
        Vector3[] weightVectors = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            weightVectors[i] = new Vector3(ContextBasedSteeringBehaviourWeights.weightVectors[i].x,
                ContextBasedSteeringBehaviourWeights.weightVectors[i].y,
                ContextBasedSteeringBehaviourWeights.weightVectors[i].z);
        }
        //find all neighbours
        List<Transform> allies = new List<Transform>();
        foreach (Collider c in surrounding)
        {
            if (c.CompareTag(allyTag) && c.gameObject != this.gameObject)
                allies.Add(c.gameObject.transform);
        }

        Vector2 targetVector = Vector3.zero;
        if (towardsTarget)
        {
            targetVector = VectorCast.CastVector3ToVector2(target.transform.position)
                - VectorCast.CastVector3ToVector2(this.transform.position);
        }
        else
        {
            targetVector = VectorCast.CastVector3ToVector2(this.transform.position)
                - VectorCast.CastVector3ToVector2(target.transform.position);
        }

        for (int i = 0; i < behaviourWeights.weights.Length; i++)
        {
            behaviourWeights.weights[i] = Vector2.Dot(VectorCast.CastVector3ToVector2(weightVectors[i]),
                targetVector.normalized) * playerBias;
            for (int j = 0; j < allies.Count; j++)
            {
                //enemy wants to separate from other enemies
                behaviourWeights.weights[i] += Vector2.Dot(VectorCast.CastVector3ToVector2(weightVectors[i]),
                (this.transform.position - allies[j].position).normalized) * enemySeparationBias;
            }
            //behaviourWeights.weights[i] /= 1 + allies.Count;

            //uncomment for turbo spam
            //Debug.Log(behaviourWeights.weights[i] + " " + i);
        }
    }

    protected virtual void Move(Transform target, bool awayFromTarget = false)
    {
        navMeshAgent.isStopped = false;
        enemyAnimator.OnRunAnimation();
        actionAvailability.SetBusy(.1f);

        Collider[] allies = Physics.OverlapSphere(transform.position, distancePrefs.minPrefferedDistanceFromAlly);
        if (awayFromTarget)
            CalculateContextWeights(target, allies, 0.5f, 2.0f, false);
        else
            CalculateContextWeights(target, allies);

        float[] weightOffsets = new float[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        Vector3 chosenDir = CalculatePath(weightOffsets);
        navMeshAgent.SetDestination(transform.position + chosenDir * 2);
    }

    // weight offsets start from Vec3.forward and going clockwise
    protected virtual Vector3 CalculatePath(float[] weightOffsets, float minWeightRange = 0.2f, float maxWeightRange = Mathf.Infinity, float maxConsideredVectors = 8)
    {
        Vector3 combinedResult = Vector3.zero;
        for (int i = 0; i < 8; i++)
        {
            //offset weights to prefer certain directions
            behaviourWeights.weights[i] += weightOffsets[i];

            if (behaviourWeights.weights[i] > minWeightRange && behaviourWeights.weights[i] < maxWeightRange)
                combinedResult += ContextBasedSteeringBehaviourWeights.weightVectors[i] * behaviourWeights.weights[i];
        }

        return combinedResult.normalized;
    }

    /// //////////////////////////////////////////////////////////////////////////////////////////

    protected void LookAt(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    protected virtual void Attack()
    {
        navMeshAgent.isStopped = true;
        float dot = Vector3.Dot((this.transform.position - target.position).normalized, this.transform.forward);
        Debug.Log(dot);
        if (dot > -.6f)
        {
            LookAt(target);
            return;
        }
        enemyAnimator.OnStopAnimation();
        enemyAnimator.OnAttackAnimation();
        actionAvailability.SetBusy(1.3f);
    }

    protected virtual void ToggleAttackHitbox(bool toggle)
    {
        Debug.Log("Attack " + toggle);
        if (toggle)
            weaponCustom.EnableDealingDamage();
        else
            weaponCustom.DisableDealingDamage();
    }

    public void GetHit(float value, DamageType damageType)
    {
        if (this == null)
            return;

        if (damageType != DamageType.Electric)
            HandleStagger();

        bool isCrit = false;
        if (damageType == weakness)
        {
            value *= 1.5f;
            isCrit = true;
        }
        hp -= value;
        if (hp <= 0)
        {
            PlayerStats.Instance.AddKill();
            Destroy(this.gameObject);
        }
          
        else
            UpdateUI(Mathf.Round(value), isCrit);
    }

    protected virtual void HandleStagger()
    {
        enemyAnimator.OnGetHitAnimation();
        actionAvailability.SetBusy(.75f);
    }

    public void UpdateUI(float value, bool crit = false)
    {
        hpBar.fillAmount = hp / maxHp;
        if (hp == maxHp)
            hpObject.SetActive(false);
        else
            hpObject.SetActive(true);

        FindPlayer();
        if (value == 0)
            return;
        TextMeshProUGUI damageText = Instantiate(damageTextPrefab, hpBar.canvas.transform);
        damageText.text = value.ToString();
        if (crit)
            damageText.color = new Color(1f, 0.5906301f, 0f);
        Destroy(damageText, 1f);
    }

    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void OnDrawGizmos()
    {
        //if (Application.isPlaying)
        //{
        //    //for (int i = 0; i < 8; i++)
        //    //{
        //    //    Gizmos.color = Color.red;
        //    //    Gizmos.DrawLine(transform.position, transform.position + (ContextBasedSteeringBehaviourWeights.weightVectors[i] * behaviourWeights.weights[i]));
        //    //}

        //    //Gizmos.color = Color.blue;
        //    //Gizmos.DrawWireSphere(this.transform.position, distancePrefs.minPrefferedDistanceFromAlly);
        //}
    }

    public void BurnMaterial()
    {
        SkinnedMeshRenderer[] smrs = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer smr in smrs)
        {
            Material mat = smr.material;
            mat.color = new Color(0.13f, 0.13f, 0.13f);
            smr.material = mat;
        }
    }
}
