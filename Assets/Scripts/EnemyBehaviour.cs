using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;
    protected Transform target;
    public LayerMask targetLayer;
    public float attackDistance;
    public float lookSpeed;
    protected bool isAttacking;
    protected Vector3 initialPosition;

    public void SetTarget(Transform newTarget)
    {
        if (newTarget) { Debug.Log("New Target: " + newTarget.name); }
        else { Debug.Log("New Target is Null"); }
        target = newTarget;
    }

    public void LookTarget()
    {
        Vector3 playerPosition = target.position;
        playerPosition.y = transform.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(playerPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
    }

    public void AttackRadius()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= attackDistance && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("InitiateAttack");
        }
    }

    public void Walk(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void SetEnemyDestination(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }
}
