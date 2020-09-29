using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCooldown : StateMachineBehaviour
{
    MeleeEnemyBehaviour enemyBehaviour;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MeleeEnemyBehaviour>().StartAttackCooldown(); ;
    }
}
