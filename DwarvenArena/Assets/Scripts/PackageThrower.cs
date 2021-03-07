using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageThrower : MonoBehaviour
{ 

    [SerializeField]
    private Transform start;

    [SerializeField]
    private GameObject packagePrefab;

    [SerializeField]
    private float minForce = 1f;
    [SerializeField]
    private float maxForce = 1f;

    private float time = 0.5f;
    private float currentTime = 0;
    private int packagesToThrow = 0;

    void Update()
    {
        if (packagesToThrow > 0)
        {
            if (currentTime < time)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                Vector3 randomAngle = new Vector3(Random.Range(0f, 90f), Random.Range(0f, 90f), Random.Range(0f, 90f));
                GameObject package = Instantiate(packagePrefab, start.position, Quaternion.Euler(randomAngle));
                Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                package.GetComponent<Rigidbody>().AddForce(direction * Random.Range(minForce,maxForce));
                currentTime = 0f;
                packagesToThrow--;
            }
        }

    }

    public void SetNumberOfPackagesToThrow(int number)
    {
        packagesToThrow = number;
    }
}
