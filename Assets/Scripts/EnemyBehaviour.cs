using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target;

    public void SetTarget(Transform newTarget)
    {
        if (newTarget) { Debug.Log("New Target: " + newTarget.name); }
        else { Debug.Log("New Target is Null"); }
        target = newTarget;
    }
}
