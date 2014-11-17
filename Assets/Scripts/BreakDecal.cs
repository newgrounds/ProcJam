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
			if (!animator.GetBool("broken") && c && c.isAttacking) {
				// you broke it!!!
				animator.SetBool("broken", true);
				
				// randomly spawn something
				int chosenAction = Random.Range(0, 100);
				
				// 50% chance of coin
				if (chosenAction < 50) {
					StartCoroutine(SpawnCoins());
				} else if (chosenAction > 90) {
					// 10% chance of book
					//TODO: spawn book
				}
				// 40% chance of nothing
			}
		}
	}
	
	public IEnumerator SpawnCoins() {
		for (int i = 0; i < 10; i++) {
			GameObject coin = Instantiate(Resources.Load("Coin")) as GameObject;
			coin.transform.parent = transform;
			coin.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
			coin.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 2;
			yield return new WaitForSeconds(0.2f);
		}
	}
}
