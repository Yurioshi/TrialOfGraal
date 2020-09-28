using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    Animator animator;
    public static bool isBlocking = false;

    public void StartUnDefend()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(UnDefend());
    }

    IEnumerator UnDefend()
    {
        animator.SetTrigger("TerminateBlock");
        yield return new WaitUntil(WaitWhileUnDefend);

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(2);
        while (animatorStateInfo.IsName("Block3") && animatorStateInfo.normalizedTime < 1f)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(2);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.2f);

        isBlocking = false;
        PlayerManager.pimba = true;
    }

    bool WaitWhileUnDefend()
    {
        return animator.GetCurrentAnimatorStateInfo(2).IsName("Block3");
    }
}
