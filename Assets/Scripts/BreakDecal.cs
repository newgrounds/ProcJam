using UnityEngine;
using System.Collections;

public class BreakDecal : Decal {
	
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnCollisionStay2D(Collision2D collider) {
		
		
		if (collider.gameObject.CompareTag("Player")) {
		Debug.Log("break collision");
			
			CharacterController c = collider.gameObject.GetComponent<CharacterController>();
			if(c.isAttacking){
					animator.SetBool("broken", true);
			}
		}
		
	}
}
