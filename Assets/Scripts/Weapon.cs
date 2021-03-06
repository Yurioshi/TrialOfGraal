﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Transform[] raycastPoints = new Transform[0];
    public float weaponDamage;
    public bool canDamage;
    public string weaponName;
    protected RaycastHit hit;

    private void Update()
    {
        if(canDamage)
        {
            CheckCollision();
        }
    }
    public void CheckCollision()
    {
        for (int i = 0; i < (raycastPoints.Length - 1); i++)
        {
            Vector3 direction = (raycastPoints[(i + 1)].position - raycastPoints[i].position).normalized;
            float distance = Vector3.Distance(raycastPoints[i].position, raycastPoints[(i + 1)].position);
            Ray ray = new Ray(raycastPoints[i].position, direction);
            bool collide = Physics.Raycast(ray, out hit, distance, enemyLayer);
            Debug.DrawRay(raycastPoints[i].position, direction);
            if(collide)
            {
                canDamage = false;
                DoDamage(hit.collider.GetComponent<Enemy>());
                break;
            }
        }
    }
    void DoDamage(Enemy enemy)
    {
        enemy.TakeDamage(weaponDamage);
    }
}
