using UnityEngine;
using System.Collections;

public class BreakDecal : Decal {
	
	private Animator animator;
	
	void Start () {
		animator = this.GetComponent<Animator>();
	}
	
	public override void OnCollisionStay2D(Collision2D collider) {
		if (collider.gameObject.CompareTag("Player")) {
			//Debug.Log("break collision");
			
			CharacterController c = collider.gameObject.GetComponent<CharacterController>();
			if (c && c.isAttacking) {
				// you broke it!!!
				animator.SetBool("broken", true);
				// TODO: randomly spawn a coin or something
				
			}
		}
	}
}
