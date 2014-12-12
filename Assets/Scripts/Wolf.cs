using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wolf : AIController {
	// distances at which behaviors will be triggered
	private float listenDist = 1.5f;
	private float attackDist = 0.6f;
	
	// lunge variables for attacking the player
	private float lungeInterval = 0.7f;
	private float lastLungeTime;
	// this is higher to create the lunge effect
	private float attackSpeed = 0.8f;
	// speed to hunt bunnies
	private float huntSpeed = 0.07f;
	
	// don't damage the player every frame
	private float damageInterval = 0.3f;
	private float lastDamageTime;
	
	public override void Start () {
		foodTarget = Vector3.zero;
		animator = this.GetComponent<Animator>();
		renderer = this.GetComponent<SpriteRenderer>();
		float randTint = Random.Range (0, 230);
		renderer.color = new Color ((255 - randTint + 70) / 255f, (255 - randTint + 35) / 255f, (255 - randTint) / 255f);
		speedMod = 3f;
		moveSpeed = 0.05f;
		
		// create behavior tree
		tree = new BehaviorTree(
			new Selector(new List<BehaviorComponent>() {
				// attack if close to bunny
				new BehaviorAction(() => {
					GameObject bunny = null;
					GameObject[] bunnies = GameObject.FindGameObjectsWithTag("Bunny");
					foreach (GameObject b in bunnies) {
						if (Vector3.Distance(
								transform.position,
								b.transform.position
							) < listenDist) {
							bunny = b;
						}
					}
					
					// if bunny is very close
					if (bunny != null && 
							Vector3.Distance(transform.position, bunny.transform.position)
							<= attackDist) {
						Vector3 target = TargetGameObject(bunny);
						
						// lunge every so often
						if (Time.time - lastLungeTime >= lungeInterval) {
							speed = new Vector3(target.x * attackSpeed, target.y * attackSpeed);
							lastLungeTime = Time.time;
						}
						return BehaviorState.SUCCESS;
					}
					// bunny is not as close, hunt Mister Bunny
					else if (bunny != null) {
						Vector3 target = TargetGameObject(bunny);
						// move quickly towards bunny
						speed = new Vector3(target.x * huntSpeed, target.y * huntSpeed);
						return BehaviorState.SUCCESS;
					} else {
						return BehaviorState.FAILURE;
					}
				}),
				// attack if too close to player
				new BehaviorAction(() => {
					GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
					if (Vector3.Distance(
							transform.position,
							player.transform.position
						) < attackDist) {
						Vector3 target = TargetGameObject(player);
						
						// lunge every so often
						if (Time.time - lastLungeTime >= lungeInterval) {
							speed = new Vector3(target.x * attackSpeed, target.y * attackSpeed);
							lastLungeTime = Time.time;
						}
						return BehaviorState.SUCCESS;
					} else {
						return BehaviorState.FAILURE;
					}
				}),
				// slow down and stalk player
				new BehaviorAction(() => {
					if (Vector3.Distance(
							transform.position,
							GameObject.FindGameObjectsWithTag("Player")[0].transform.position
						) < listenDist) {
						GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
						Vector3 target = TargetGameObject(player);
						speed = new Vector3(target.x * moveSpeed / 2, target.y * moveSpeed / 2);
						// TODO: growl
						return BehaviorState.SUCCESS;
					} else {
						return BehaviorState.FAILURE;
					}
				}),
				// eat nearby food
				new BehaviorAction(() => {
					if (foodTarget != Vector3.zero) {
						TargetFood();
						return BehaviorState.SUCCESS;
					} else {
						return BehaviorState.FAILURE;
					}
				}),
				// wander
				new BehaviorAction(() => {
					Wander();
					return BehaviorState.SUCCESS;
				})
			})
		);
	}
	
	public override void OnTriggerStay2D(Collider2D col) {
		base.OnTriggerStay2D(col);
		// damage the player
		if (col.CompareTag("Player") && Time.time - lastDamageTime >= damageInterval) {
			col.gameObject.GetComponent<CharacterController>().DecreaseHealth(10);
			lastDamageTime = Time.time;
		}
	}
	
	public override void OnCollisionEnter2D(Collision2D col) {
		base.OnCollisionEnter2D(col);
		// eat the bunny
		if (col.gameObject.CompareTag("Bunny")) {
			Destroy(col.gameObject);
		}
	}
}
