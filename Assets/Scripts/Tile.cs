using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	
	public int sortingOrder;
	public float height;
	public float height2;
	public float height3;
	private Decal decal;
	public Vector3 origin;
	
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
}
