using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
	public GameObject camera;
	public static List<Tile> tiles = new List<Tile> ();
	public List<Sprite> sprites;
	// Use this for initialization
	void Start ()
	{
		int mapWidth = 50;
		for (int x = 0; x < mapWidth; x++) {
			for (int y = 0; y < mapWidth; y++) {
				float height = SimplexNoise.Noise.Generate (x / 12f, y / 12f) / 2f;
				float height2 = SimplexNoise.Noise.Generate (x + 1000 / 8f, y + 1000 / 8f) / 2f;
				float height3 = SimplexNoise.Noise.Generate (x / 15f, y / 15f) / 2f;
				
				GameObject tile;
				/*
				if (height3 < 0f) {
					
					tile = Instantiate (Resources.Load ("water")) as GameObject;
					tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, 0);
					//tile.GetComponent<SpriteRenderer>().color = new Color(1f + height,1f + height,1f + height, 1);	
					tile.GetComponent<SpriteRenderer> ().sortingOrder = y;
					tile.GetComponent<Animator>().speed = Random.Range(.1f,.6f);
					tile.GetComponent<Tile> ().sortingOrder = y;
					tile.GetComponent<Tile> ().height = 0;
					tile.GetComponent<SpriteRenderer> ().color = new Color (1f + height, 1f + height, 1f + height, 1f + height/2f);
					
					tiles.Add (tile.GetComponent<Tile> ());
				} else if (height3 > 0f) {
				*/
					tile = Instantiate (Resources.Load ("tile")) as GameObject;
					tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f + height, 0);
					tile.GetComponent<SpriteRenderer> ().color = new Color (1f + height, 1f + height, 1f + height, 1);
					tile.GetComponent<SpriteRenderer> ().sortingOrder = y;
					tile.GetComponent<Tile> ().sortingOrder = y;
					tile.GetComponent<Tile> ().height = height;
					Tile tileObject = tile.GetComponent<Tile> ();
					tiles.Add (tileObject);

					if (height2 > .3f) {
						float randomSize = Random.Range (-.5f, .5f);
						float xOffset = 0;//Random.Range(-.5f, .5f) ;
						GameObject tree = Instantiate (Resources.Load ("pine")) as GameObject;
						tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
						tree.transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y + .2f, 0);
						tree.GetComponent<SpriteRenderer> ().sortingOrder = y + 1;	
						tileObject.decal = tree;
						tree.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
					
					}
					else if (height > .25f) {
						GameObject tree;
						if (Random.Range (0, 10) > 8) {
							tree = Instantiate (Resources.Load ("deadTree")) as GameObject;
						} else {
							tree = Instantiate (Resources.Load ("tree")) as GameObject;
							tree.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
						
						}
						float randomSize = Random.Range (-.5f, .5f);
						tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
						tree.transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y + .2f, 0);
						tree.GetComponent<SpriteRenderer> ().sortingOrder = y + 1;	
						tileObject.decal = tree;
						tree.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);
					
					} else if (height > .2f) {
						GameObject grass = Instantiate (Resources.Load ("grass")) as GameObject;
						grass.transform.position = new Vector3 (tile.transform.position.x, tile.transform.position.y + .2f, 0);
						grass.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);
						grass.GetComponent<SpriteRenderer> ().sortingOrder = y + 1;	
					}
					
				//}
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
