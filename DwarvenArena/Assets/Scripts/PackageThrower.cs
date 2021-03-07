using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageThrower : MonoBehaviour
{
    [SerializeField]
    private Transform start;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private GameObject packagePrefab;

    [SerializeField]
    private float force = 1f;

    private float time = 2;
    private float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime < time)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            Vector3 randomAngle = new Vector3(Random.Range(0f, 90f), Random.Range(0f, 90f), Random.Range(0f, 90f));
            GameObject package = Instantiate(packagePrefab, start.position, Quaternion.Euler(randomAngle));
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            package.GetComponent<Rigidbody>().AddForce(direction * force);
            currentTime = 0f;
        }

    }
}
