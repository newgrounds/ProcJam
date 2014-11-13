﻿using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
	public GameObject camera;
	public static List<Tile> tiles = new List<Tile> ();
	public static List<Tile> tilesWithoutDecals = new List<Tile>();
	public static List<Ruins> ruins = new List<Ruins>();
	public List<Sprite> sprites;
	private int mapWidth = 0;
	
	// Use this for initialization
	void Start ()
	{

		mapWidth = 50;
		for (int x = 0; x < mapWidth; x++) {
			for (int y = 0; y < mapWidth; y++) {
				float height = SimplexNoise.Noise.Generate (x / 12f, y / 12f) / 2f;
				float height2 = SimplexNoise.Noise.Generate (x + 1000 / 8f, y + 1000 / 8f) / 2f;
				float height3 = SimplexNoise.Noise.Generate (x / 15f, y / 15f) / 2f;
				GameObject tile;
				/*
				if (height3 < -.4f) {
					
					tile = Instantiate (Resources.Load ("water")) as GameObject;
					tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, 0);
					//tile.GetComponent<SpriteRenderer>().color = new Color(1f + height,1f + height,1f + height, 1);	
					tile.GetComponent<SpriteRenderer> ().sortingOrder = y;
					tile.GetComponent<Animator>().speed = Random.Range(.1f,.6f);
					tile.GetComponent<Tile> ().sortingOrder = y;
					tile.GetComponent<Tile> ().height = 0;
					tile.GetComponent<SpriteRenderer> ().color = new Color (1f + height, 1f + height, 1f + height, 1f + height/2f);
					
					tiles.Add (tile.GetComponent<Tile> ());
				} else if (height3 > -.4f) {
			*/
				tile = Instantiate (Resources.Load ("tile")) as GameObject;
				tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f + height, 0);
				tile.GetComponent<SpriteRenderer> ().color = new Color (1f + height, 1f + height, 1f + height, 1);
				tile.GetComponent<SpriteRenderer> ().sortingOrder = y;
				
				Tile tileObject = tile.GetComponent<Tile> ();
				tileObject.sortingOrder = y;
				tileObject.height = height;
				tileObject.height2 = height2;
				tileObject.height3 = height3;
				tileObject.origin = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, 0);
				tiles.Add(tileObject);
				
		//}
			}
		}
		
		// generate ruins
		for (int i = 0; i < 8; i++) {
			ruins.Add(new Ruins (mapWidth, Random.Range(0, tiles.Count)));
		}
		
		// remove any empty ruins
		for (int i = 0; i < ruins.Count; i++) {
			if (ruins[i].floors.Count == 0) {
				ruins.Remove(ruins[i]);
			}
		}

		foreach (Tile t in tiles) {
			float height = t.height;
			float height2 = t.height2;
			float height3 = t.height3;
			if (t.GetDecal() == null) {
				if (height2 > .3f) {
					float randomSize = Random.Range (-.5f, .5f);
					float xOffset = 0;//Random.Range(-.5f, .5f) ;
					GameObject tree = Instantiate (Resources.Load ("pine")) as GameObject;
					tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
					tree.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y + .2f, 0);
					tree.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 1;
					tree.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
					t.SetDecal(tree.GetComponent<Decal>());
				
				} else if (height > .25f) {
					GameObject tree;
					if (Random.Range (0, 10) > 8) {
						tree = Instantiate (Resources.Load ("deadTree")) as GameObject;
					} else {
						tree = Instantiate (Resources.Load ("tree")) as GameObject;
						tree.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
					
					}
					float randomSize = Random.Range (-.5f, .5f);
					tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
					tree.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y + .2f, 0);
					tree.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + + 1;	
					tree.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);
					t.SetDecal(tree.GetComponent<Decal>());
				
				} else if (height > .2f) {
					GameObject grass = Instantiate (Resources.Load ("grass")) as GameObject;
					grass.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y + .2f, 0);
					grass.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);
					grass.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + + 1;	
				}	
			}
		}
		
		// get a list of tiles without decals
		foreach (Tile t in tiles) {
			if (t.GetDecal() == null) {
				tilesWithoutDecals.Add(t);
			}
		}
	}
	

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		/*
		foreach(Tile t in tiles){
			Vector2 tilePos = new Vector2(t.transform.position.x,t.transform.position.y);
			Vector2 camPos = new Vector2(camera.transform.position.x,camera.transform.position.y);
			if(Vector2.Distance(camPos,tilePos) > 2){
				t.enabled = false;	
				Debug.Log("out");
			}
			else{
				t.enabled = true;	
			}
		}
		*/
	}
}
