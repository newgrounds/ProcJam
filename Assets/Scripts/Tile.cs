using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	
	public int sortingOrder;
	public float height;
	public float height2;
	public float height3;
	public Decal decal;
	public Vector3 origin;
	public Vector2 posn;
	public Vector2 offsetPosn;
	
	
	public Decal GetDecal() {
		return decal;
	}
	
	public void SetDecal(Decal d) {
		if (d == null) {
			decal = null;
		} else {
			d.tile = this;
			decal = d;
		}
	}
	
	public void FixedUpdate(){
		float h = SimplexNoise.Noise.Generate ((offsetPosn.x - posn.x + TerrainGenerator.globalTimer/50f) / 12f, (offsetPosn.y + posn.y) / 12f, 0) / 2f;//Mathf.FloorToInt(height)/5f;
		this.GetComponent<SpriteRenderer> ().color = new Color (1f + h, 1f + h, 1f + h, 1);
		
		
	}
}
