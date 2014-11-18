using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
		private Animator animator;
		private SpriteRenderer renderer;
		private float direction = 0;
		private Vector2 speed;
		private Tile currentTile;
		private float lastHeight = 0;
		private int timer = 0;
		public float speedMod;

	
		void Start ()
		{
				animator = this.GetComponent<Animator> ();
				renderer = this.GetComponent<SpriteRenderer> ();
				float randTint = Random.Range (0, 230);
				renderer.color = new Color ((255 - randTint + 70) / 255f, (255 - randTint + 35) / 255f, (255 - randTint) / 255f);
				speedMod = 3f;
		}
		
		private void Wander()
		{
			if (timer > 50) {
				timer = 0;
				speed = new Vector2 (Random.Range(-.01f, .01f), Random.Range(-.01f, .01f));
			}
		}

		private void AdjustDirection () 
		{
		if (speed.y > 0)
			direction = 2;
		else if (speed.y < 0)
			direction = 0;
			if (speed.x > 0) 
					direction = 1;
			else if (speed.x < 0)
						direction = 3;
	}
		
		void FixedUpdate ()
		{		
				timer++;
				Wander ();
				AdjustDirection ();
				if (speed.magnitude <= .001f) {
						speed = Vector2.zero;
				}
				if (speed.magnitude <= .1f) {
						animator.Play ("Idle");
				}
				
				// fix character sorting order
				foreach (TerrainChunk c in TerrainGenerator.spawnedChunks) {
						foreach (Tile t in c.tiles) {
								if (Vector3.Distance (t.transform.position + (Vector3.up * 0.5f), transform.position) < .25f) {
										renderer.sortingOrder = t.sortingOrder + 2;
								}
						}
				}
				animator.SetFloat ("Direction", direction);
				animator.SetFloat ("Speed", speed.magnitude * 100f);
				//Debug.Log(speed.magnitude);
		transform.Translate (new Vector3 (speed.x/speedMod, speed.y/speedMod, 0));
		}

		
}
	
	
