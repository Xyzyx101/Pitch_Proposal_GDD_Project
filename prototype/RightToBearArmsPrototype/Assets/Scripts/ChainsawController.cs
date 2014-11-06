using UnityEngine;
using System.Collections;

public class ChainsawController : MonoBehaviour {
	public bool attacking = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (attacking) {
			collider.gameObject.SendMessage("Kill");
		}
	}
}
