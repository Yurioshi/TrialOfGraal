using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    WeaponManager weaponManager;
    MeleeAttack meleeAttack;
    RangedAttack rangedAttack;
    Block block;
    public static bool pimba = true;

    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        meleeAttack = GetComponent<MeleeAttack>();
        rangedAttack = GetComponent<RangedAttack>();
        animator = GetComponent<Animator>();
        block = GetComponent<Block>();
    }

    void Update()
    {
        weaponManager.SearchWeapon();

        //Equip Logic
        if (Input.GetKeyDown(KeyCode.E) && !MeleeAttack.isAttacking && !Block.isBlocking && !RangedAttack.isCharging && !RangedAttack.isAiming)
        {
            if(weaponManager.lastSeenArrow)
            {
                if (!weaponManager.lastSeenArrow.inAir)
                {
                    rangedAttack.arrowQuantity++;
                    Destroy(weaponManager.lastSeenArrow.gameObject);
                    weaponManager.lastSeenArrow = null;
                }
            }
            if(weaponManager.lastSeenWeapon)
            {
                if (weaponManager.actualWeapon)
                {
                    if (weaponManager.lastSeenWeapon.weaponName != "Escudo")
                    {
                        if (weaponManager.hasShield) { weaponManager.Unequip(weaponManager.shieldWeapon); }
                        weaponManager.Unequip(weaponManager.actualWeapon);
                        weaponManager.Equip(weaponManager.lastSeenWeapon);
                        weaponManager.SetWeaponAnimation();
                    }
                    else
                    {
                        if (weaponManager.actualWeapon.weaponName == "Espada" || weaponManager.actualWeapon.weaponName == "Machado")
                        {
                            weaponManager.Equip(weaponManager.lastSeenWeapon);
                            weaponManager.SetWeaponAnimation();
                        }
                    }
                }
                else if (weaponManager.lastSeenWeapon.weaponName != "Escudo")
                {
                    weaponManager.Equip(weaponManager.lastSeenWeapon);
                    weaponManager.SetWeaponAnimation();
                }
            }
        }
            
        
        //Unequip Logic
        else if (Input.GetKeyDown(KeyCode.Q) && !MeleeAttack.isAttacking && !Block.isBlocking && !RangedAttack.isCharging && !RangedAttack.isAiming)
        {
            if (weaponManager.actualWeapon)
            {
                weaponManager.Unequip(weaponManager.actualWeapon);
                weaponManager.SetWeaponAnimation();
            }
            if (weaponManager.hasShield) { weaponManager.Unequip(weaponManager.shieldWeapon); }
        }

        //Attack Logic
        else if (Input.GetMouseButtonDown(0))
        {
            if(WeaponManager.weaponClass != WeaponClass.none && WeaponManager.weaponClass != WeaponClass.BowArrow)
            {
                if (!MeleeAttack.isAttacking && !Movement.isInAir && !Block.isBlocking)
                {
                    MeleeAttack.isAttacking = true;
                    meleeAttack.attackCombo = 0;
                    meleeAttack.lastAttack = false;
                    meleeAttack.StartMeleeAttack("Attack");
                }
                else if (MeleeAttack.isAttacking && meleeAttack.nextAttackReady)
                {
                    meleeAttack.nextAttack = true;
                }
            }
            else if(WeaponManager.weaponClass == WeaponClass.BowArrow && rangedAttack.arrowQuantity > 0)
            {
                if(!RangedAttack.isCharging && !RangedAttack.isAiming)
                {
                    rangedAttack.StartCharge();
                }
            }
        }

        //Block Logic
        else if (WeaponManager.weaponClass == WeaponClass.AxeShield || WeaponManager.weaponClass == WeaponClass.SwordShield)
        {
            if(weaponManager.hasShield)
            {
                if (Input.GetMouseButton(1) && !Block.isBlocking && !MeleeAttack.isAttacking && !Movement.isInAir)
                {
                    animator.SetTrigger("Block");
                    Block.isBlocking = true;
                }
                else if (!Input.GetMouseButton(1) && Block.isBlocking && pimba)
                {
                    pimba = false;
                    block.StartUnDefend();
                }
            }
        }
    }
}