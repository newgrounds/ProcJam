using UnityEngine;

public class Decal : MonoBehaviour {
	public Tile tile;
	public GameObject child;
	public virtual void OnCollisionStay2D(Collision2D collider){}
}