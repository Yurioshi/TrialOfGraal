using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Transform[] raycastPoints = new Transform[0];
    int originalDamage;
    public int weaponDamage;
    public bool canDamage;
    public string weaponName;
    protected RaycastHit hit;

    private void Awake()
    {
        originalDamage = weaponDamage;
    }

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
            Debug.DrawRay(raycastPoints[i].position, direction, Color.red);
            float distance = Vector3.Distance(raycastPoints[i].position, raycastPoints[(i + 1)].position);
            Ray ray = new Ray(raycastPoints[i].position, direction);
            bool collide = Physics.Raycast(ray, out hit, distance, enemyLayer);
            
            if(collide)
            {
                canDamage = false;
                DoDamage(hit.collider.GetComponent<EntityManager>());
                break;
            }
        }
    }
    void DoDamage(EntityManager enemy)
    {
        enemy.TakeDamage(weaponDamage);
    }
    public void ResetWeaponDamage()
    {
        weaponDamage = originalDamage;
    }
}
