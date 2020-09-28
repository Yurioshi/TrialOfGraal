using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    Animator animator;
    public GameObject flechaPrefab;
    public Flecha currentArrow;
    public Transform shootLocation;
    public Transform hand;
    public int arrowQuantity = 10;
    public static bool isCharging = false;
    public static bool isAiming = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartCharge()
    {
        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        isCharging = true;
        animator.SetTrigger("Charge");

        currentArrow = Instantiate(flechaPrefab, hand).GetComponent<Flecha>();
        yield return new WaitUntil(WaitUntilCharge);

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(5);
        while (animatorStateInfo.IsName("Charge") && animatorStateInfo.normalizedTime < 1f)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(5);
            Debug.Log("IsCharging");
            if (!Input.GetMouseButton(0))
            {
                Debug.Log("CancelCharge");
                animator.SetTrigger("CancelCharge");
                isCharging = false;
                Destroy(currentArrow.gameObject);
                yield return new WaitForEndOfFrame();
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        if(isCharging)
        {
            isCharging = false;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        isAiming = true;
        arrowQuantity--;
        animator.SetTrigger("Aim");
        
        yield return new WaitUntil(WaitUntilAim);
        
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(5);
        while (animatorStateInfo.IsName("AimLocomotion"))
        {
            if (!Input.GetMouseButton(0))
            {
                animator.SetTrigger("Shoot");
                currentArrow.transform.parent = null;
                currentArrow.rb.velocity = Vector3.zero;
                currentArrow.rb.isKinematic = false;
                currentArrow.rb.AddForce(transform.TransformDirection(Vector3.forward) * 50f, ForceMode.Impulse);
                currentArrow.inAir = true;
                yield return new WaitForEndOfFrame();
                break;
            }
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(5);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        isAiming = false;
    }

    bool WaitUntilCharge()
    {
        return animator.GetCurrentAnimatorStateInfo(5).IsName("Charge");
    }

    bool WaitUntilAim()
    {
        return animator.GetCurrentAnimatorStateInfo(5).IsName("AimLocomotion");
    }
}
