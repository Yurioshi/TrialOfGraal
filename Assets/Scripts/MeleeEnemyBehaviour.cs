﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyBehaviour : EnemyBehaviour
{
    public float attackCooldown;
    public bool isCooldown = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        MeleeAttackLogic();
    }

    void MeleeAttackLogic()
    {
        bool isWalking = false;
        if (target)
        {
            animator.applyRootMotion = true;
            navMeshAgent.enabled = false;
            if (!isCooldown) { AttackRadius(); }
            if (!isAttacking)
            {
                LookTarget();
                isWalking = true;
            }
        }
        else
        {
            animator.applyRootMotion = false;
            navMeshAgent.enabled = true;
            SetEnemyDestination(initialPosition);
            if (Vector3.Distance(transform.position, initialPosition) > 0.2f) { isWalking = true; }
        }
        Walk(isWalking);
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