using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    public Rigidbody rb;
    //public Transform[] arrowRaycastPointsParent;
    //public Transform[] arrowRaycastPoints1;
    //public Transform[] arrowRaycastPoints2;
    public bool inAir = false;
    public bool shootedByPlayer = true;
    //RaycastHit hit;

    private void Awake()
    {
        //arrowRaycastPoints1 = GetComponentsOnlyInChildren(arrowRaycastPointsParent[0]);
        //arrowRaycastPoints2 = GetComponentsOnlyInChildren(arrowRaycastPointsParent[1]);

        rb = GetComponent<Rigidbody>();
        //inAir = true;
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
            if (other.gameObject.layer != 9) //&& other.gameObject.layer != 10)
            {
                if(shootedByPlayer && other.gameObject.layer != 10 || !shootedByPlayer && other.gameObject.layer != 11)
                {
                    Debug.Log(other.name);
                    inAir = false;
                    rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                    rb.isKinematic = true;
                    if (other.gameObject.layer == 8)
                    {
                        other.GetComponent<Enemy>().TakeDamage(1);
                    }
                }
            }
        }
    }

    /*
    private bool InAirBehaviour(Transform[] arrowRaycastPoints)
    {
        for (int i = 0; i < (arrowRaycastPoints.Length - 1); i++)
        {
            Vector3 direction = (arrowRaycastPoints[(i + 1)].position - arrowRaycastPoints[i].position).normalized;
            float distance = Vector3.Distance(arrowRaycastPoints[i].position, arrowRaycastPoints[(i + 1)].position);
            Debug.DrawRay(arrowRaycastPoints[i].position, direction);
            if (Physics.Raycast(arrowRaycastPoints[i].position, direction, out hit, distance) && inAir)
            {
                inAir = false;
                rb.Sleep();
                rb.isKinematic = true;
                return true;
            }
        }

        return false;
    }

    private Transform[] GetComponentsOnlyInChildren(Transform parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        Transform[] firstChildren = new Transform[parent.childCount];
        int index = 0;
        foreach (Transform child in children)
        {
            if (child.parent == parent)
            {
                firstChildren[index] = child;
                index++;
            }
        }
        return firstChildren;
    }
    */
}
