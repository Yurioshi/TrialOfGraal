using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCooldown : StateMachineBehaviour
{
    SwordEnemyBehaviour enemyBehaviour;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<SwordEnemyBehaviour>().StartAttackCooldown(); ;
    }
}
