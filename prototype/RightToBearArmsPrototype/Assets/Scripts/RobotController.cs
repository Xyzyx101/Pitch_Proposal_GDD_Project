using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour {
	public float maxSpeed = 7f;
	float damage = 3.5f;
	float damageInterval = 0.25f;
	Animator myAnimator;
	GameObject bear;
	bool grabbing;
	float grabbingTimer;
	bool dying;
	float deathTimer = 1.0f;
	bool facingRight = true;
	
	void Start () {
		grabbing = false;
		dying = false;
		myAnimator = GetComponent<Animator> ();
		bear = GameObject.Find ("bear");
		if (bear.transform.position.x < transform.position.x) {
			maxSpeed *= -1f;
		}
	}
	
	
	void Update () {
		if (maxSpeed < 0 && facingRight) {
			Flip ();
		} else if(maxSpeed > 0 && !facingRight) {
			Flip ();
		}
		if (grabbing) {
			grabbingTimer -= Time.deltaTime;
			if (grabbingTimer < 0) {
				if (!dying) {
					bear.GetComponent<BearController>().TakeDamage(damage);
				}
				grabbingTimer = damageInterval;
			}
		}
		if (dying) {
			deathTimer -= Time.deltaTime;
			if (deathTimer < 0f) {
				Destroy(this.gameObject);
			}
		}
	}
	
	void FixedUpdate () {
		if (!dying) {
			rigidbody2D.velocity = new Vector2 (maxSpeed, rigidbody2D.velocity.y);
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		if (dying) return;
		if (grabbing) return;
		if (collision.gameObject.CompareTag ("bear")) {
			maxSpeed *= 3;
			myAnimator.SetTrigger ("grab");
			if (!dying) {
				bear.GetComponent<BearController>().TakeDamage(damage);
			}
			grabbing = true;
			grabbingTimer = damageInterval;
		}
	}

	void Kill() {
		grabbing = false;
		if (!dying) {
			dying = true;
			myAnimator.SetTrigger("die");
			rigidbody2D.velocity = new Vector2 (0f, 0f);
			KillColliders();
		}
	}

	void Flip () {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1f;
		transform.localScale = theScale;
	}

	void KillColliders() {
		Collider2D[] colliders = GetComponents<Collider2D> ();
		foreach (Collider2D col in colliders) {
			col.enabled = false;
		}
	}
}
