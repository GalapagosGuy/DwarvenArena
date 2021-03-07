using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrozenPrison : MonoBehaviour
{
    public GameObject icePrison;
    public GameObject icePrisonFractured;

    [SerializeField]
    private float prisonTime = 3.0f;

    private float currentTime = 0.0f;

    private GameObject target;
    private GameObject createdIce = null;

    public void CreatePrison(GameObject target)
    {
        this.target = target;
        target.GetComponent<NavMeshAgent>().enabled = false;
        target.GetComponentInChildren<Animator>().enabled = false;

        //createdIce = Instantiate(icePrison, icePrison.transform.position, Quaternion.identity);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= prisonTime)
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        if (target)
        {
            target.GetComponent<NavMeshAgent>().enabled = true;
            target.GetComponentInChildren<Animator>().enabled = true;
        }

        DestroyPrison();
    }

    public void DestroyPrison()
    {
        GameObject go = Instantiate(icePrisonFractured, this.transform.position, this.transform.rotation);

        Destroy(go, 2.0f);

        //if (createdIce)
        //    Destroy(createdIce);

        PrisonManager.Instance.RemovePrison(this);
        Destroy(this.gameObject);
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
