using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormieIdleBehaviour : StateMachineBehaviour
{
	public float seeRange = 10f;

	Transform player;
	Rigidbody2D rb;
	Wormie wormie;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponentInParent<Rigidbody2D>();
        wormie = animator.GetComponentInParent<Wormie>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       	if (Vector2.Distance(player.position, rb.position) <= seeRange)
		{
			animator.SetBool("PlayerIsNear", true);
		}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }


}
