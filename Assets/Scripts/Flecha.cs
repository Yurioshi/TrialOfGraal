using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    public Rigidbody rb;
    public bool inAir = false;
    public bool shootedByPlayer = true;
    public int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));
        if(inAir)
        {
            if (rb.velocity != Vector3.zero) { rb.transform.rotation = Quaternion.LookRotation(rb.velocity); }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(inAir)
        {
            if (shootedByPlayer && other.gameObject.layer == 8 || !shootedByPlayer && other.gameObject.layer == 10)
            {
                Debug.Log(other.name);
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                other.GetComponent<EntityManager>().TakeDamage(damage);
            }
            else if (other.gameObject.layer != 9)
            {
                inAir = false;
                rb.isKinematic = true;
            }
        }
    }
}
