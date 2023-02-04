using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdie : MonoBehaviour
{
    private Transform player;

	public bool isFlipped = false;
	private Rigidbody2D rb;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
		rb = GetComponent<Rigidbody2D>();
    }

	public void MoveForward(float speed) {
		Vector3 pos = rb.position;

		var direction = -transform.right;
		if (isFlipped)
			direction = -transform.right;

		pos += direction * 1.0f;

		Vector2 target = new Vector2(pos.x, rb.position.y);

		Debug.DrawLine(rb.position, target, Color.cyan, 0.0f, false);

		Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
		rb.MovePosition(newPos);

		DetectWall();
	}

	public void Flip()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else 
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

	void DetectWall() {
		Vector3 pos = transform.position;
		pos -= transform.right * 3.0f;

		LayerMask mask = LayerMask.GetMask("Ground Hard", "Ground Soft");

		Debug.DrawLine(transform.position, pos, Color.green, 0.1f, false);
		Debug.DrawLine(pos, pos - transform.right * 1.0f, Color.red, 0.0f, false);

		Collider2D colInfo = Physics2D.OverlapCircle(pos, 1.0f, mask);
		if (colInfo != null)
		{
			Debug.DrawLine(transform.position, colInfo.gameObject.transform.position, Color.yellow, 1.0f, false);
			Flip();
		}
	}

	public void LookAtPlayer() {
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}
}
