using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : CastedSpell
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float damage = 50.0f;

    private Vector3 targetPosition;

    public override void Initialize(Vector3 source, Vector3 target)
    {
        targetPosition = target;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

        if (transform.position == targetPosition)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject != PlayerController.Instance.gameObject)
        {
            Debug.Log(other.gameObject.name);
            //Damage
            Destroy(this.gameObject);
        }
    }
}
