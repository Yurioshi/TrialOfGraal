using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBehaviour : StateMachineBehaviour
{
    RaycastHit hit;
    public LayerMask targetLayer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<RangerEnemyBehaviour>().DoCoroutine(Aim(animator));
    }

    IEnumerator Aim(Animator animator)
    {
        RangerEnemyBehaviour rangerEnemy = animator.GetComponent<RangerEnemyBehaviour>();
        yield return new WaitForSeconds(3);
        bool aiming = true;
        while(aiming)
        {
            Ray ray = new Ray(rangerEnemy.arco.transform.position, Vector3.forward);
            Debug.DrawRay(rangerEnemy.arco.transform.position, Vector3.forward);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.CompareTag("Player"))
                {
                    aiming = false;
                    animator.SetTrigger("Shoot");
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
