using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public float health;
	float maxHealth = 100f;
	SpriteRenderer renderer;
	
	void Start () {
		renderer = GetComponent<SpriteRenderer> ();
	}

	void Update () {
		float healthRatio = health / maxHealth;
		transform.localScale = new Vector3 (healthRatio, 1f, 1f);
		renderer.color = Color.Lerp (Color.red, Color.green, healthRatio);
	}
}
