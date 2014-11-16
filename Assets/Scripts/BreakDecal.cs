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
				
				// randomly spawn something
				int chosenAction = Random.Range(0, 100);
				
				// 50% chance of coin
				if (chosenAction < 50) {
					string type = "Coin";
					GameObject coin = Instantiate(Resources.Load(type)) as GameObject;
					coin.transform.parent = transform;
					coin.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
					coin.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 2;
				} else if (chosenAction > 90) {
					// 10% chance of book
					//TODO: spawn book
				}
				// 40% chance of nothing
			}
		}
	}
}
