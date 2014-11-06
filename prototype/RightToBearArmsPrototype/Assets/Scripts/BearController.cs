using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {
	public float maxSpeed = 15f;
	bool facingLeft = true;
	public float hp = 100f;

	public GameObject[] chainsaws;
	ChainsawController chainsawController01;
	ChainsawController chainsawController02;

	bool grounded = false;
	bool jumping = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	float landDelay;
	float maxLandDelay = 0.5f;

	bool attacking;
	float maxAttackTimer = 0.5f;
	float attackTimer;

	Animator myAnimator;

	void Start () {
		myAnimator = GetComponent<Animator> ();
		chainsawController01 = chainsaws [0].GetComponent<ChainsawController> ();
		chainsawController02 = chainsaws [1].GetComponent<ChainsawController> ();
	}

	void Update () {
		if (Input.GetKeyDown ("up") && grounded) {
			Jump ();
		}
		landDelay -= Time.deltaTime;
		if (Input.GetKeyDown ("space")) {
			if(Random.value > 0.5f) {
				myAnimator.SetTrigger("attack01");
			} else {
				myAnimator.SetTrigger("attack02");
			}
			attacking = true;
			attackTimer = maxAttackTimer;
		}
		if (attacking) {
			attackTimer -= Time.deltaTime;
			if (attackTimer < 0f) {
				attacking = false;
			}
		}
		chainsawController01.attacking = attacking;
		chainsawController02.attacking = attacking;

		if (hp <= 0) {
			Destroy(gameObject);
		}
	}

	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");

		myAnimator.SetFloat ("speed", Mathf.Abs (move));

		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

		if (move < 0 && !facingLeft) {
			Flip ();
		} else if(move > 0 && facingLeft) {
			Flip ();
		}

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		if (grounded && jumping) {
			if(landDelay < 0) {
				myAnimator.SetTrigger("land");
			}
		}
	}

	void Jump() {
		jumping = true;
		myAnimator.SetTrigger ("jump");
		rigidbody2D.AddForce(new Vector2(0f, 40000f));
		landDelay = maxLandDelay;
	}

	void Flip () {
		facingLeft = !facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1f;
		transform.localScale = theScale;
	}

	public void TakeDamage(float damage) {
		hp -= damage;
		if (hp < 0) {
			Destroy (this.gameObject);
		}
	}
}
