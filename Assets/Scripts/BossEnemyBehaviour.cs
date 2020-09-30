using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyBehaviour : EnemyBehaviour
{
    int quantityToSpawn = 50;
    public Vector3 localToSpawnMax;
    public Vector3 localToSpawnMin;
    public GameObject[] pedregulhoPrefabs;
    bool isSpawningPedregulhos = false;
    EntityManager entityManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        entityManager = GetComponent<EntityManager>();
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false;
    }

    private void Update()
    {
        bool isWalking = false;

        if (target)
        {
            if(!isAttacking)
            {
                float distance = Vector3.Distance(target.position, transform.position);
                Vector3 rayOrigin = transform.position;
                rayOrigin.y = 1f;
                Vector3 direction = transform.TransformDirection(Vector3.forward);
                if (!isCooldown)
                {
                    ChooseAttack(false);
                }
                else if (distance <= attackDistance && Physics.Raycast(rayOrigin, direction, 10f, targetLayer))
                {
                    ChooseAttack(true);
                }

                LookTarget();
                isWalking = true;
            }
        }

        Walk(isWalking);
    }

    void ChooseAttack(bool isNormal)
    {
        isAttacking = true;

        animator.SetBool("NormalAttack", isNormal);
        animator.SetTrigger("Attack");

        if (isNormal)
        {
            StartCoroutine(NormalAttack());
        }
        else
        {
            StartCoroutine(EarthquakeAttack());
        }
    }

    IEnumerator NormalAttack()
    {
        yield return new WaitUntil(NormalAttackBegin);

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (animatorStateInfo.IsName("NormalAttack") && animatorStateInfo.normalizedTime < 1f)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForEndOfFrame();
        }

        animator.SetTrigger("TerminateAttack");

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    bool NormalAttackBegin()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack");
    }

    
    IEnumerator EarthquakeAttack()
    {
        yield return new WaitUntil(EarthquakeAttackBegin);

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (animatorStateInfo.IsName("EarthquakeAttack") && animatorStateInfo.normalizedTime < 1f)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(SpawnPedregulhos());
        StartCoroutine(Heal());

        animator.SetTrigger("TerminateAttack");

        yield return new WaitForSeconds(.5f);

        isCooldown = true;
        isAttacking = false;

        while (isSpawningPedregulhos) { yield return new WaitForEndOfFrame(); }

        yield return new WaitForSeconds(attackCooldown);

        isCooldown = false;
    }

    bool EarthquakeAttackBegin()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("EarthquakeAttack");
    }

    bool HealBegin()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("LifeHealing");
    }

    IEnumerator Heal()
    {
        while(isSpawningPedregulhos)
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            entityManager.TakeDamage(-1);
        }
    }

    IEnumerator SpawnPedregulhos()
    {
        isSpawningPedregulhos = true;
        for (int i = 0; i < quantityToSpawn; i++)
        {
            Vector3 localToSpawn = RandomPosition(localToSpawnMin, localToSpawnMax);
            localToSpawn.y = 15f;

            GameObject pedregulho = Instantiate(pedregulhoPrefabs[Random.Range(0, pedregulhoPrefabs.Length)], localToSpawn, Quaternion.identity);
            pedregulho.name = "Pedregulho" + (i + 1);

            yield return new WaitForSeconds(.2f);
        }
        isSpawningPedregulhos = false;
    }

    Vector3 RandomPosition(Vector3 min, Vector3 max)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        float z = Random.Range(min.z, max.z);

        return new Vector3(x, y, z);
    }

    void CanDamage()
    {
        weapon.canDamage = true;
    }

    void CantDamage()
    {
        weapon.canDamage = false;
    }
}
