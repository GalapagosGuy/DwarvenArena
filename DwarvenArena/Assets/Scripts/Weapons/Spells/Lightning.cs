using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : CastedSpell
{
    public Transform overlapBoxCenter;

    [SerializeField] private float damage;
    [SerializeField] private float damageOffset;
    [SerializeField] private DamageType damageType;
    [SerializeField] private float interval;
    [SerializeField] private float chancesForCritical = 10.0f;
    [SerializeField] private float criticalDamage = 30.0f;

    private float currentIntervalTime = 0.0f;

    public GameObject lightningCritical;

    public float RandomDamage()
    {
        return Random.Range((int)(damage - damageOffset), (int)(damage + damageOffset) + 1);
    }

    public override void Initialize(Vector3 source, Vector3 target)
    {
        PlayerController.Instance?.GetComponent<PlayerSlots>().AddSpellHearingForStopCasting(this);

        this.transform.parent = PlayerController.Instance.GetComponent<PlayerSlots>().LeftHand.transform;
    }

    public override void OnCastingSpellStop()
    {
        PlayerController.Instance?.GetComponent<PlayerSlots>().RemoveSpellHearingForStopCasting(this);
        Destroy(this.gameObject);
    }

    private List<GameObject> targets = new List<GameObject>();

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<Structure>())
            return;

        IHitable iHitable = other.GetComponentInParent<IHitable>();

        if (iHitable != null && iHitable != PlayerController.Instance.GetComponent<IHitable>() && !targets.Contains(other.transform.root.gameObject))
        {
            targets.Add(other.transform.root.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IHitable iHitable = other.GetComponentInParent<IHitable>();

        if (iHitable != null && targets.Contains(other.transform.root.gameObject))
        {
            targets.Remove(other.transform.root.gameObject);
        }
    }

    private void Update()
    {
        if (!PlayerStats.Instance.HasEnoughMana(cost))
            PlayerStats.Instance.playerSlots.StopUsingSpell();


        this.transform.forward = PlayerController.Instance.transform.forward;
        //this.transform.rotation = Quaternion.Euler(PlayerController.Instance.transform.forward + Vector3.up);

        currentIntervalTime += Time.deltaTime;

        if (currentIntervalTime >= interval)
        {
            GameObject[] targetsToDealDamage = targets.ToArray();

            foreach (GameObject go in targetsToDealDamage)
            {
                if (go == null)
                    continue;

                IHitable iHitable = go.GetComponent<IHitable>();

                if (iHitable != null)
                    iHitable.GetHit(RandomDamage(), damageType);

                if (Random.Range(0, 100.0f) < chancesForCritical)
                {
                    iHitable.GetHit(criticalDamage, damageType);

                    GameObject lightningCrit = Instantiate(lightningCritical, transform.position, Quaternion.identity);
                    lightningCrit.transform.GetChild(0).transform.position = go.transform.position;
                    lightningCrit.transform.GetChild(1).transform.position = go.transform.position + new Vector3(0.0f, 20.0f, 0.0f);
                    Destroy(lightningCrit, 0.2f);
                }
            }

            currentIntervalTime = 0.0f;
        }
        PlayerStats.Instance.SubstractMana(cost);
    }
}
