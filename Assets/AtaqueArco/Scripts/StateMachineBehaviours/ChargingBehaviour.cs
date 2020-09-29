using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingBehaviour : StateMachineBehaviour
{
    OldRangerEnemyBehaviour enemyBehaviour;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBehaviour = animator.gameObject.GetComponent<OldRangerEnemyBehaviour>();
        if(!enemyBehaviour.arco.hasArrow)
        {
            enemyBehaviour.Recharge();
        }
        enemyBehaviour.Charge();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!enemyBehaviour.isCharging)
        {
            animator.SetBool("IsCharging", enemyBehaviour.isCharging);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBehaviour = null;
    }
}
