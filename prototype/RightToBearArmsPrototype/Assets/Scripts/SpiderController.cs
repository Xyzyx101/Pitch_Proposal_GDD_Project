﻿using UnityEngine;
using System.Collections;

public class SpiderController : MonoBehaviour {
	public float maxSpeed = 5f;
	float damage = 30;
	Animator myAnimator;
	GameObject bear;
	bool dying;

	void Start () {
		dying = false;
		myAnimator = GetComponent<Animator> ();
		bear = GameObject.Find ("bear");
		if (bear.transform.position.x < transform.position.x) {
			maxSpeed *= -1f;
		}
	}

	void Update () {
	}

	void FixedUpdate () {
		rigidbody2D.velocity = new Vector2 (maxSpeed, rigidbody2D.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag ("bear")) {
			if(!dying) {
				dying = true;
				bear.GetComponent<BearController>().TakeDamage(damage);
			}
			StartCoroutine(Explode());
		}
	}
	
	IEnumerator Explode() {
		myAnimator.SetTrigger("explode");
		yield return new WaitForSeconds(1.5f);
		Destroy (this.gameObject);
	}

	void Kill() {
		dying = true;
		StartCoroutine (Explode ());
	}
}
