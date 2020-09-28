using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float life = 100;

    public void TakeDamage(float damageAmount)
    {
        life -= damageAmount;
        Debug.LogWarning(life);
        Material tempMaterial = GetComponent<MeshRenderer>().material;
        tempMaterial.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<MeshRenderer>().material = tempMaterial;
    }
}
