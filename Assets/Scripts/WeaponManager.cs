using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WeaponClass
{
    none = 1, SwordShield = 2, AxeShield = 2, SoloSword = 3, SoloAxe = 4,  BowArrow = 5
}

public class WeaponManager : MonoBehaviour
{
    public Animator animator;
    public LayerMask weaponLayer;
    public Weapon actualWeapon;
    public Weapon shieldWeapon;
    public Weapon lastSeenWeapon;
    public Flecha lastSeenArrow;
    public float lastSeenWeaponDist;
    public float lastSeenArrowDist;
    public float pickUpRadius = 2;
    public static WeaponClass weaponClass = WeaponClass.none;
    public Transform[] weaponPositions = new Transform[0];
    public bool hasShield;
    public Collider[] teste;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWeaponAnimation()
    {
        WeaponClass newWeaponClass = WeaponClass.none;
        if(actualWeapon)
        {
            switch (actualWeapon.weaponName)
            {
                case "Espada":
                    if (hasShield)
                    {
                        newWeaponClass = WeaponClass.SwordShield;
                        Equip(actualWeapon);
                    }
                    else
                    {
                        newWeaponClass = WeaponClass.SoloSword;
                    }
                    break;
                case "Machado":
                    if (hasShield)
                    {
                        newWeaponClass = WeaponClass.AxeShield;
                        Equip(actualWeapon);
                    }
                    else
                    {
                        newWeaponClass = WeaponClass.SoloAxe;
                    }
                    break;
                case "Arco":
                    newWeaponClass = WeaponClass.BowArrow;
                    break;
            }
        }

        animator.SetLayerWeight((int)weaponClass, 0f);
        animator.SetLayerWeight((int)newWeaponClass, 1f);
        weaponClass = newWeaponClass;
    }

    public void SearchWeapon()
    {
        if (lastSeenArrow && teste.Length > 0)
        {
            bool isSeenArrow = false;
            for (int i = 0; i < teste.Length; i++)
            {
                if (teste[i] == lastSeenArrow)
                {
                    isSeenArrow = true;
                    Debug.DrawLine(transform.position, lastSeenArrow.transform.position, Color.red);
                }

                if (i == (teste.Length - 1) && !isSeenArrow)
                {
                    lastSeenArrow = null;
                }
            }
        }

        Collider[] seenWeapons = Physics.OverlapCapsule(transform.position, (transform.position + (Vector3.up * 100)), pickUpRadius, weaponLayer); //Sphere(transform.position, pickUpRadius, weaponLayer);
        teste = seenWeapons;
        if (seenWeapons.Length != 0)
        {
            for (int i = 0; i < seenWeapons.Length; i++)
            {
                Weapon weapon = seenWeapons[i].GetComponent<Weapon>();
                Flecha arrow = seenWeapons[i].GetComponent<Flecha>();
                
                if (weapon)
                {
                    if(weapon == actualWeapon)
                    {
                        seenWeapons[i] = null;
                    }
                    else if(lastSeenWeapon)
                    {
                        if(lastSeenWeapon != actualWeapon && lastSeenWeapon != weapon)
                        {
                            lastSeenWeaponDist = Vector3.Distance(transform.position, lastSeenWeapon.transform.position);
                            float weaponDist = Vector3.Distance(transform.position, weapon.transform.position);
                            if(weaponDist < lastSeenWeaponDist) { lastSeenWeapon = weapon; }
                        }
                    }
                    else { lastSeenWeapon = weapon; }
                }
                else if (arrow && !RangedAttack.isAiming && !RangedAttack.isCharging)
                {
                    if(lastSeenArrow)
                    {
                        if (lastSeenArrow != arrow)
                        {
                            lastSeenArrowDist = Vector3.Distance(transform.position, lastSeenArrow.transform.position);
                            float arrowDist = Vector3.Distance(transform.position, arrow.transform.position);
                            if (arrowDist < lastSeenArrowDist) { lastSeenArrow = arrow; }
                        }
                    }
                    else
                    {
                        lastSeenArrow = arrow;
                    }
                }
            }
        }

        if (lastSeenWeapon) { Debug.DrawLine(transform.position, lastSeenWeapon.transform.position, Color.green); if (Vector3.Distance(transform.position, lastSeenWeapon.transform.position) > 2f) { lastSeenWeapon = null; } }
        
    }

    public void Equip(Weapon weaponToEquip)
    {
        if (weaponToEquip.weaponName == "Escudo")
        {
            if(hasShield)
            {
                Unequip(shieldWeapon);
            }
            hasShield = true;
            shieldWeapon = weaponToEquip;
        }
        else
        {
            actualWeapon = weaponToEquip;
        }

        int i = 0;
        switch (weaponToEquip.weaponName)
        {
            case "Espada":
                if (hasShield)
                {
                    i = 4;
                }
                else
                {
                    i = 0;
                }
                break;
            case "Machado":
                if (hasShield)
                {
                    i = 3;
                }
                else
                {
                    i = 5;
                }
                break;
            case "Arco":
                i = 1;
                break;
            case "Escudo":
                weaponToEquip.transform.localScale = Vector3.one;
                i = 2;
                break;
        }

        lastSeenWeapon = null;
        weaponToEquip.transform.parent = weaponPositions[i];
        weaponToEquip.transform.localPosition = Vector3.zero;
        weaponToEquip.transform.localRotation = Quaternion.identity;
        weaponToEquip.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Unequip(Weapon weaponToUnequip)
    {
        weaponToUnequip.transform.parent = null;
        weaponToUnequip.GetComponent<Rigidbody>().isKinematic = false;

        if(weaponToUnequip.weaponName != "Escudo")
        {
            actualWeapon = null;
        }
        else
        {
            shieldWeapon = null;
            hasShield = false;
        }
    }
}