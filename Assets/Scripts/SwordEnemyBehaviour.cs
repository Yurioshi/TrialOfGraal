using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordEnemyBehaviour : EnemyBehaviour
{
    public LayerMask targetLayer;
    public float attackRadius;
    public float attackCooldown;
    public bool isAttacking = false;
    public bool isCooldown = false;
    public int speed = 5;
    Animator animator;
    Vector3 initialPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        if(target)
        {
            animator.applyRootMotion = true;
            navMeshAgent.enabled = false;
            AttackRadius();
            if (!isAttacking)
            {
                LookTarget();
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        }
        else
        {
            animator.applyRootMotion = false;
            navMeshAgent.enabled = true;
            SetEnemyDestination(initialPosition);
            if(navMeshAgent.velocity.magnitude > 0f)
            {
                animator.SetBool("IsWalking", true);
            }
            else
            { animator.SetBool("IsWalking", false); }
        }
    }

    void SetEnemyDestination(Vector3 position)
    {
        
        navMeshAgent.SetDestination(position);
    }

    void LookTarget()
    {
        Vector3 playerPosition = target.position;
        playerPosition.y = transform.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(playerPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
    void AttackRadius()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= attackRadius && !isAttacking && !isCooldown)
        {
            isAttacking = true;
            animator.SetTrigger("InitiateAttack");
        }
    }

    public void StartAttackCooldown()
    {
        StartCoroutine(AttackCooldown(attackCooldown));
    }

    IEnumerator AttackCooldown(float cooldown)
    {
        isAttacking = false;
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
        animator.SetTrigger("CooldownOver");
    }
}
