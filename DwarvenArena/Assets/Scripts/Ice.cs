using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField] private float appearingSpeed;

    public GameObject iceFractured;

    private MeshRenderer renderer;

    private void Start()
    {
        appearingSpeed = Random.Range(0.75f, 1.5f);

        renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (renderer.material.GetFloat("Display") > -1.0f)
            renderer.material.SetFloat("Display", renderer.material.GetFloat("Display") - (Time.deltaTime * appearingSpeed));
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private bool damageDealt = false;

    private void OnTriggerEnter()
    {
        if (damageDealt == false)
        {
            GameObject iceFracturedGO = Instantiate(iceFractured, this.transform.position, Quaternion.identity);
            GetComponentInParent<BlizzardOnTheGround>().DealDamage();
            damageDealt = true;
            Destroy(this.gameObject);
        }
    }
}
