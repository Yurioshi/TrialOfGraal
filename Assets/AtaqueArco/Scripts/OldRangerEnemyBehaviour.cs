using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldRangerEnemyBehaviour : MonoBehaviour
{
    public GameObject flechaPrefab;
    public Arco arco;
    public bool isCharging = false;

    private void Start()
    {
        GetComponent<Animator>().SetBool("IsCharging", true);
        GetComponent<Animator>().SetTrigger("InitiateAttack");
    }

    public void Shoot()
    {
        arco.Shoot();
        StartAttack();
    }

    void StartAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Animator>().SetTrigger("InitiateAttack");
    }

    public void Recharge()
    {
        arco.RechargeBow(flechaPrefab);
    }

    public void Charge()
    {
        Debug.Log("pei");
        isCharging = !arco.ChargeShoot();
        StartCoroutine(ChargeBow());
    }

    IEnumerator ChargeBow()
    {
        while (isCharging)
        {
            isCharging = !arco.ChargeShoot();
            yield return new WaitForEndOfFrame();
        }
    }

    public void DoCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
