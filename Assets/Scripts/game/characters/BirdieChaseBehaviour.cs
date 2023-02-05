using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdieChaseBehaviour : StateMachineBehaviour
{
	public float speed = 2.5f;
	public float attackRange = 3f;
    public float seeRange = 10f;

	Transform player;
	Rigidbody2D rb;
	Birdie birdie;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponentInParent<Rigidbody2D>();
        birdie = animator.GetComponentInParent<Birdie>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        birdie.LookAtPlayer();

		Vector2 target = new Vector2(player.position.x, player.position.y);
		Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
		rb.MovePosition(newPos);

		if (Vector2.Distance(rb.position, player.position) <= attackRange)
		{
			animator.SetTrigger("Attack");
		}

        if (Vector2.Distance(rb.position, player.position) > seeRange)
		{
			animator.SetBool("PlayerIsNear", false);
		}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }


}
