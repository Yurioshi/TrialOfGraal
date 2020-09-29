using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public int life = 100;
    public int maxLife;
    public HealthBarController HBR;

    private void Awake()
    {
        maxLife = life;
    }

    public void TakeDamage(int damageAmount)
    {
        life -= damageAmount;
        HBR.OnHealthChange(life);
        Debug.LogWarning(life);
        if(life <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
