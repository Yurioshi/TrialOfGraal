using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemyBehaviour : EnemyBehaviour
{
    public LayerMask targetLayer;
    public float attackRadius;
    public float attackCooldown;
    public bool isAttacking = false;
    public bool isCooldown = false;
    public int speed = 5;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(target) { AttackRadius(); if (!isAttacking) { LookTarget(); } }
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
