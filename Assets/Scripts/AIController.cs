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
	private int decisionThreshold = 0;
	public float speedMod;
	private bool waiting = false;
	private int waitTimer = 0;
	
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		renderer = this.GetComponent<SpriteRenderer> ();
		float randTint = Random.Range (0, 230);
		renderer.color = new Color ((255 - randTint + 70) / 255f, (255 - randTint + 35) / 255f, (255 - randTint) / 255f);
		speedMod = 3f;
	}
		
	private void Wander ()
	{
		if (timer > decisionThreshold) {
			if(!waiting && Random.Range(0,10) > 8){
				waiting = true;
				waitTimer = Random.Range(0,100);
			}
			if(waiting && timer < decisionThreshold + waitTimer){
				speed = Vector2.zero;
				animator.Play ("Idle");
			}
			else{
				timer = 0;
				decisionThreshold = Random.Range(0, 300);
				speed = new Vector2 (Random.Range (-.035f, .035f), Random.Range (-.035f, .035f));
			}
		}
	}

	private void AdjustDirection ()
	{
		if (speed.y > 0 && speed.y > Mathf.Abs(speed.x)){
			direction = 2;
		}
		if (speed.x > 0 && speed.x > Mathf.Abs(speed.y)){
			direction = 1;
		}
		if (speed.y < 0 && Mathf.Abs(speed.y) > Mathf.Abs(speed.x)){
			direction = 0;
		}
		if (speed.x < 0 && Mathf.Abs(speed.x) > Mathf.Abs(speed.y)){
			direction = 3;
		}
	}
		
	void FixedUpdate ()
	{		
		timer++;
		Wander ();
		AdjustDirection ();
		if (speed.magnitude <= .01f) {
			speed = Vector2.zero;
		}
		
		if (speed.magnitude <= .01f) {
			animator.Play ("Idle");
		}
		
				
		// fix character sorting order
		foreach (TerrainChunk c in TerrainGenerator.spawnedChunks) {
			foreach (Tile t in c.tiles) {
				if (Vector3.Distance (t.transform.position + (Vector3.up * 0.1f), transform.position) < .25f) {
					renderer.sortingOrder = t.sortingOrder + 2;
				}
			}
		}
		animator.SetFloat ("Direction", direction);
		animator.SetFloat ("Speed", speed.magnitude * 100f);
		//Debug.Log(speed.magnitude);
		transform.Translate (new Vector3 (speed.x / speedMod, speed.y / speedMod, 0));
	}

		
}
	
	
