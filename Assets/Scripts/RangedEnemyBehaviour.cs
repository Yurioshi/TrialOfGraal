using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyBehaviour : EnemyBehaviour
{
    public GameObject flechaPrefab;
    public Flecha currentArrow;
    public Transform hand;
    public bool isCharging = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        RangedAttackLogic();
    }

    void RangedAttackLogic()
    {
        bool isWalking = false;

        if (target)
        {
            animator.applyRootMotion = true;
            navMeshAgent.enabled = false;

            if(Vector3.Distance(transform.position, target.position) < attackDistance)
            {
                if (!isCharging)
                {
                    StartCoroutine(Charge());
                }
            }
            else
            {
                isWalking = true;
            }

            LookTarget();

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

    IEnumerator Charge()
    {
        isCharging = true;
        animator.SetTrigger("Charge");

        currentArrow = Instantiate(flechaPrefab, hand).GetComponent<Flecha>();
        yield return new WaitUntil(WaitUntilCharge);

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (animatorStateInfo.IsName("IsCharging") && animatorStateInfo.normalizedTime < 1f)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(attackCooldown);

        animator.SetBool("IsAiming", false);
        animator.SetTrigger("Shoot");

        Shoot();

        yield return new WaitForSeconds(2f);

        isCharging = false;
    }

    void Shoot()
    {
        currentArrow.transform.parent = null;
        currentArrow.rb.velocity = Vector3.zero;
        currentArrow.rb.isKinematic = false;
        currentArrow.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        currentArrow.rb.AddForce(transform.TransformDirection(Vector3.forward) * 100f, ForceMode.Impulse);
        currentArrow.shootedByPlayer = false;
        currentArrow.inAir = true;
    }

    bool WaitUntilCharge()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("IsCharging");
    }
}
