using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormie : MonoBehaviour
{
    private Transform player;
	[SerializeField] Transform _attackPoint;

	private bool isFlipped = false;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	public void LookAtPlayer()
	{
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

	public void Attack() {
		SoundSystem.PlaySound(Sounds.Instance.GetAudioClip("wormattack"));
		LayerMask mask = LayerMask.GetMask("Player");
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_attackPoint.position, 1.0f); // add layer mask
            foreach (var enemy in hitColliders)
            {
                var takeDamage = enemy.GetComponent<TakeDamage>();
                if (takeDamage != null)
                    takeDamage.Hit(13);
            }         

	}
}
