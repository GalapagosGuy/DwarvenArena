using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplash : MonoBehaviour
{
    private bool countdownToDeath = false;
    private float timeToDeath;
    private float currentTimeToDeath = 0.0f;

    public void SetTimeToDestroy(float time)
    {
        countdownToDeath = true;
        timeToDeath = time;
    }

    public void Update()
    {
        if (countdownToDeath)
        {
            currentTimeToDeath += Time.deltaTime;

            if (currentTimeToDeath >= timeToDeath)
            {
                GetComponent<Animator>()?.SetTrigger("destroyTrigger");
            }
        }
    }

    public void DestroyByAnim()
    {
        Destroy(this.gameObject);
    }
}
