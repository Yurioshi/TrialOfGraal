using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Animator animator;
    public WeaponManager weaponManager;
    public static bool isAttacking = false;
    public bool nextAttackReady = false;
    public bool nextAttack;
    public int attackCombo = 0;
    public bool lastAttack = false;
    public int attackLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator Attack(string triggerName)
    {
        animator.SetTrigger(triggerName);
        yield return new WaitUntil(WaitUntilAttack);

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo((int)WeaponManager.weaponClass);
        while (animatorStateInfo.IsName("Attack" + attackCombo) && animatorStateInfo.normalizedTime < 1f)
        {
            Debug.Log("Attack" + attackCombo + ": " + animatorStateInfo.normalizedTime);
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo((int)WeaponManager.weaponClass);
            yield return new WaitForEndOfFrame();
        }

        if(nextAttack)
        {
            nextAttackReady = false;
            nextAttack = false;
            StartMeleeAttack("NextAttack");
        }
        else
        {
            animator.SetTrigger("EndAttack");

            if (!lastAttack)
            {
                yield return new WaitUntil(WaitUntilAttackEnd);

                animatorStateInfo = animator.GetCurrentAnimatorStateInfo((int)WeaponManager.weaponClass);
                while (animatorStateInfo.IsName("EndAttack" + attackCombo) && animatorStateInfo.normalizedTime < 1f)
                {
                    Debug.Log("EndAttack" + attackCombo + ": " + animatorStateInfo.normalizedTime);
                    animatorStateInfo = animator.GetCurrentAnimatorStateInfo((int)WeaponManager.weaponClass);
                    yield return new WaitForEndOfFrame();
                }
            }

            nextAttackReady = false;
            yield return new WaitForSeconds(0.2f);
            isAttacking = false;
        }
    }

    public void StartMeleeAttack(string triggerName)
    {
        if(attackCombo == 1) { weaponManager.actualWeapon.ResetWeaponDamage(); }
        attackCombo++;
        weaponManager.actualWeapon.weaponDamage *= attackCombo;
        StartCoroutine(Attack(triggerName));
    }

    bool WaitUntilAttack()
    {
        return animator.GetCurrentAnimatorStateInfo((int)WeaponManager.weaponClass).IsName("Attack" + attackCombo);
    }
    bool WaitUntilAttackEnd()
    {
        return animator.GetCurrentAnimatorStateInfo((int)WeaponManager.weaponClass).IsName("EndAttack" + attackCombo);
    }
    void NextAttackReady()
    {
        nextAttackReady = true;
    }
    public void CanDamage()
    {
        weaponManager.actualWeapon.canDamage = true;
    }
    public void CantDamage()
    {
        weaponManager.actualWeapon.canDamage = false;
    }
    public void LastAttack()
    {
        lastAttack = true;
    }
}