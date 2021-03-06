using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : CastedSpell
{
    public Transform overlapBoxCenter;

    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;
    [SerializeField] private float interval;

    private float currentIntervalTime = 0.0f;

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

    private List<IHitable> targets = new List<IHitable>();

    private void OnTriggerStay(Collider other)
    {
        IHitable iHitable = other.GetComponentInParent<IHitable>();

        if (iHitable != null && iHitable != PlayerController.Instance.GetComponent<IHitable>() && !targets.Contains(iHitable))
        {
            targets.Add(iHitable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IHitable iHitable = other.GetComponentInParent<IHitable>();

        if (iHitable != null && targets.Contains(iHitable))
        {
            targets.Remove(iHitable);
        }
    }

    private void Update()
    {
        this.transform.forward = PlayerController.Instance.transform.forward;
        //this.transform.rotation = Quaternion.Euler(PlayerController.Instance.transform.forward + Vector3.up);

        currentIntervalTime += Time.deltaTime;

        if (currentIntervalTime >= interval)
        {
            IHitable[] targetsToDealDamage = targets.ToArray();

            foreach (IHitable iHitable in targetsToDealDamage)
            {
                iHitable.GetHit(damage, damageType);
            }

            currentIntervalTime = 0.0f;
        }
    }
}
