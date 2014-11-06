using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {
	public float maxSpeed = 15f;
	bool facingLeft = true;
	public float hp = 100f;

	Animator myAnimator;

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

	string lastAttack;

	HealthBar healthBar;

	void Start () {
		myAnimator = GetComponent<Animator> ();
		chainsawController01 = chainsaws [0].GetComponent<ChainsawController> ();
		chainsawController02 = chainsaws [1].GetComponent<ChainsawController> ();
		healthBar = GetComponentInChildren<HealthBar> ();
	}

	void Update () {
		if (Input.GetKeyDown ("up") && grounded) {
			Jump ();
		}
		if (Input.GetKey ("down") && grounded) {
			myAnimator.SetBool("duck", true);
		} else {
			myAnimator.SetBool("duck", false);		
		}
		landDelay -= Time.deltaTime;
		if (Input.GetKeyDown ("space")) {
			string thisAttack = "attack01";
			while(thisAttack == lastAttack) {
				float myNumber = Random.Range(0f, 3f);
				if(myNumber < 1f) {
					thisAttack = "attack01";
				} else if(myNumber < 2f) {
					thisAttack = "attack02";
				} else {
					thisAttack = "attack03";
				}
			}
			lastAttack = thisAttack;
			myAnimator.SetTrigger(thisAttack);
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
			Die ();
		}
		healthBar.health = hp;
	}

	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");
		if (Input.GetKey ("down")) {
			move = 0f;
		}
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
			Die ();
		}
	}

	void Die() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_WEBPLAYER
		Application.OpenURL(webplayerQuitURL);
		#else
		Application.Quit();
		#endif
	}
}
