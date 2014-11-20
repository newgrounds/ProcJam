using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainChunk : MonoBehaviour {
	public GameObject camera;
	public List<Tile> tiles = new List<Tile> ();
	public List<Tile> tilesWithoutDecals = new List<Tile> ();
	public List<Ruins> ruins = new List<Ruins> ();
	public List<Sprite> sprites;
	public static int mapWidth = 10;
	public static float tileSize = 0.5f;
	private int ruinsToSpawn = 3;
	Transform player;
	public bool containsPlayer;
	public Vector2 normalizedPos;
	private float offsetX;
	private float offsetY;
	private float randZ;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
		
		normalizedPos = new Vector2(
			(TerrainGenerator.terrainOrigin.x - transform.position.x) / (mapWidth * tileSize),
			(TerrainGenerator.terrainOrigin.y - transform.position.y) / (mapWidth * tileSize)
		);
		
		// first chunk
		GenerateChunk();
	}
	
	void Update() {
		// if player is in bounds of terrain
		containsPlayer = Contains(player.transform);
		
		// check if we should be deleted
		if (!containsPlayer && !TerrainGenerator.validChunkPosns.Contains(transform.position)) {
			TerrainGenerator.spawnedChunks.Remove(this);
			Destroy(gameObject);
		}
	}
	
	public bool Contains(Transform trans) {
		return trans.position.x >= transform.position.x &&
		trans.position.x <= (transform.position.x + (mapWidth * tileSize)) &&
		trans.position.y <= transform.position.y &&
		trans.position.y >= (transform.position.y - (mapWidth * tileSize));
	}
	
	void GenerateChunk() {
		// random seed
		//float randZ = Random.Range (0, 100000);
		randZ = TerrainGenerator.randZ;
		
		offsetX = normalizedPos.x * mapWidth;
		offsetY = normalizedPos.y * mapWidth;
		float thresh = .1f;
		
		// loop to generate tiles
		for (int x = 0; x < mapWidth; x++) {
			for (int y = 0; y < mapWidth; y++) {

				
				float height = SimplexNoise.Noise.Generate ((offsetX - x) / 12f, (offsetY + y) / 12f, randZ) / 2f;
				float height2 = SimplexNoise.Noise.Generate ((offsetX - x) + 1000 / 8f, (offsetY + y) + 1000 / 8f, randZ) / 2f;
				float height3 = SimplexNoise.Noise.Generate ((offsetX - x) / 15f, (offsetY + y) / 15f, randZ) / 2f;
				int tileOrder = (y + (int)offsetY) + (10 * (int)offsetY);
				
				//string waterFolder = "Water/";
	
				CreateTileLayer(x,y, .1f, -2f, "Water/", TerrainGenerator.GetWaterNoise);
				CreateTileLayer(x,y, .175f, -2f, "Sand/", TerrainGenerator.GetSandNoise);
				//CreateTileLayer(x,y, .3f, -.3f, "Dirt/", TerrainGenerator.GetDirtNoise);
				
				//CreateTileLayer(x,y, .1f, "Water/");
					
				if (TerrainGenerator.GetLandNoise(offsetX - x,offsetY + y, TerrainGenerator.randZ) >= -2f) {
					GameObject tile;
					tile = Instantiate (Resources.Load ("tile")) as GameObject;
					tile.transform.parent = transform;
					//tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f + height, 0);
					tile.GetComponent<SpriteRenderer> ().sortingOrder = tileOrder;
					Tile tileObject = tile.GetComponent<Tile> ();
					tileObject.posn = new Vector2(x,y);
					tileObject.offsetPosn = new Vector2(offsetX,offsetY);
					tileObject.sortingOrder = tileOrder;
					tileObject.height = height;
					tileObject.height2 = height2;
					tileObject.height3 = height3;
					tileObject.origin = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, 0);
					tiles.Add (tileObject);
				
					float h = TerrainGenerator.GetLandNoise  ((offsetX - x) / 1f, (offsetY + y) / 1f, randZ) * 1f;
					tileObject.geoHeight = h;
					//Color c = new Color ((200f/255f) + h, 1f - h/10f, 0, 1);
					Color c = new Color((200f/255f),1,0,1);
					tileObject.color = c;
					tileObject.GetComponent<SpriteRenderer> ().color = c;
					tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, 0);
				
				
				}
				
			}
		}
		
		// generate ruins
		//for (int i = 0; i < ruinsToSpawn; i++) {
		/*
		ruins.Add (new Ruins (mapWidth, Random.Range (0, 0), this, 0));
		
		if(Random.Range(0,10) > 5){
			Vector3 bunnyPos = tiles[Random.Range(0,tiles.Count)].transform.position;
			GameObject bunny = Instantiate (Resources.Load ("Animals/MisterBunny")) as GameObject;
			bunny.transform.position = bunnyPos;
			bunny.transform.parent = transform;
		}
		
		//}
		
		// remove any empty ruins
		for (int i = 0; i < ruins.Count; i++) {
			if (ruins [i].floors.Count == 0) {
				ruins.Remove (ruins [i]);
			}
		}
		*/
		
		
		// tree generation
		foreach (Tile t in tiles) {
			float height = t.height;
			float height2 = t.height2;
			float height3 = t.height3;
			float geoHeight = t.geoHeight;
			if (t.GetDecal () == null) {
				if (geoHeight > .5f) {
					float randomSize = Random.Range (-.5f, .5f);
					float xOffset = 0;//Random.Range(-.5f, .5f) ;
					
					GameObject tree;
					
					if(t.height3 > 0f){
						tree = Instantiate (Resources.Load ("tempTree")) as GameObject;
					}
					else{
						tree = Instantiate (Resources.Load ("pine")) as GameObject;
					}
					tree.transform.parent = transform;
					tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
					tree.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, -2);
					tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 2;	
					
					tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
					t.SetDecal (tree.GetComponent<Decal> ());
				
				} 
				/*
				if (height2 > .35f && geoHeight > thresh) {
					float randomSize = Random.Range (-.5f, .5f);
					float xOffset = 0;//Random.Range(-.5f, .5f) ;
					
					GameObject tree;
					
					if(t.height3 > 0f){
						tree = Instantiate (Resources.Load ("tempTree")) as GameObject;
					}
					else{
						tree = Instantiate (Resources.Load ("pine")) as GameObject;
					}
					tree.transform.parent = transform;
					tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
					tree.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, -2);
					tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 2;	
					
					tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
					t.SetDecal (tree.GetComponent<Decal> ());
				
				} else if (height > .3f && geoHeight > thresh) {
					GameObject tree;
					if (Random.Range (0, 10) > 8) {
						tree = Instantiate (Resources.Load ("deadTree")) as GameObject;
					} else {

						if(t.geoHeight > .1f){
							tree = Instantiate (Resources.Load ("oldTree")) as GameObject;
						}
						else{
							tree = Instantiate (Resources.Load ("tree")) as GameObject;
						}
						
						tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
					
					}
					tree.transform.parent = transform;
					float randomSize = Random.Range (-.5f, .5f);
					tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
					tree.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, -2);
					tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 2;	
					tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);
					t.SetDecal (tree.GetComponent<Decal> ());
				
				} else if (height > .2f && geoHeight > thresh) {
					
					GameObject grass = Instantiate (Resources.Load ("grass")) as GameObject;
					grass.transform.parent = transform;
					grass.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, -1);
					grass.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);
					grass.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 2;	
					
				}	
				*/
			}
			
		}
		

		/*
		foreach (Tile t in tiles) {
			float height = t.height;
			float height2 = t.height2;
			float height3 = t.height3;
			if (t.GetDecal () == null) {
				if (height3 > .1f) {
					string type = "Coin";
					//if(Random.Range(0,10) > 9) type = "gem"; else type = "Coin";
					GameObject coin = Instantiate (Resources.Load (type)) as GameObject;
					coin.transform.parent = transform;
					//tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
					coin.transform.position = new Vector3 (t.transform.position.x, t.transform.position.y, 0);
					coin.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 3;
					//tree.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
					//t.SetDecal (coin.GetComponent<Decal> ());
				
				} 
			}
		}
		*/
		// get a list of tiles without decals
		foreach (Tile t in tiles) {
			if (t.GetDecal() == null) {
				tilesWithoutDecals.Add(t);
			}
		}	
	}
	
	void PrintValidTile(string type, float val, float thresh){
		Debug.Log(type + ": " + val + " / " + thresh + ", " + (val < thresh));		
	}
	
	void CreateTileLayer(int x, int y, float thresh, float minThresh, string rootFolder, TerrainGenerator.GetGlobalNoise f){

		GameObject tile;
		int tileOrder = (y + (int)offsetY) + (10 * (int)offsetY);
		float center = f(offsetX - x,offsetY + y, randZ);
		float above = f(offsetX - x,offsetY + y - 1, randZ);
		float below = f(offsetX - x,offsetY + y + 1, randZ);
		float left = f(offsetX - x - 1,offsetY + y, randZ);
		float right = f(offsetX - x + 1,offsetY + y, randZ);
		
		float topRight = f(offsetX - x + 1,offsetY + y - 1, randZ);
		float topLeft = f(offsetX - x - 1,offsetY + y - 1, randZ);
		
		float bottomRight = f(offsetX - x + 1,offsetY + y + 1, randZ);
		float bottomLeft = f(offsetX - x - 1,offsetY + y + 1, randZ);
		
		

		if ( center > minThresh && center < thresh) {
			/*
			PrintValidTile("center", center, thresh);
			PrintValidTile("above", above, thresh);
			PrintValidTile("below", below, thresh);
			PrintValidTile("left", left, thresh);
			PrintValidTile("right", right, thresh);
			PrintValidTile("topRight", topRight, thresh);
			PrintValidTile("topLeft", topLeft, thresh);
			PrintValidTile("bottomRight", bottomRight, thresh);
			PrintValidTile("bottomLeft", bottomLeft, thresh);
			*/
			
			//LEGENDARY SOLO 4 SPLIT
			if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft >= thresh && bottomRight >= thresh
				&& topLeft >= thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "4Split")) as GameObject;
			}
			
			//4split with gap
			else if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft >= thresh && bottomRight >= thresh
				&& topLeft >= thresh && topRight < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "4SplitGapXYFlip")) as GameObject;
			}	
			else if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft >= thresh && bottomRight >= thresh
				&& topLeft < thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "4SplitGapYFlip")) as GameObject;
			}	
			else if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft < thresh && bottomRight >= thresh
				&& topLeft >= thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "4SplitGap")) as GameObject;
			}	
			else if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft >= thresh && bottomRight < thresh
				&& topLeft >= thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "4SplitGapXFlip")) as GameObject;
			}	
			//tsplit
			//left/right
			else if(above < thresh && left >= thresh && right < thresh && below < thresh && bottomRight >= thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "tsplitXFlip")) as GameObject;
			}
			else if(above < thresh && right >= thresh && left < thresh && below < thresh && bottomLeft >= thresh && topLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "tsplit")) as GameObject;
			}	
			//top/bottom
			else if(above < thresh && right < thresh && left < thresh && below >= thresh && topLeft >= thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "tsplit90")) as GameObject;
			}	
			else if(above >= thresh && right < thresh && left < thresh && below < thresh && bottomLeft >= thresh && bottomRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "tsplitYFlip90")) as GameObject;
			}			
			
			//squeezes
			//above
			else if(above >= thresh && left < thresh && right < thresh && below < thresh && bottomRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "squeezeXFlip90")) as GameObject;
			}
			else if(above >= thresh && left < thresh && right < thresh && below < thresh && bottomLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "squeezeXYFlip90")) as GameObject;
			}
			//below
			else if(above < thresh && left < thresh && right < thresh && below >= thresh && topLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "squeezeYFlip90")) as GameObject;
			}
			else if(above < thresh && left < thresh && right < thresh && below >= thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "squeeze90")) as GameObject;
			}
			
			
			//left
			else if(above < thresh && left >= thresh && right < thresh && below < thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "squeezeXFlip")) as GameObject;
			}
			else if(above < thresh && left >= thresh && right < thresh && below < thresh && bottomRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "squeezeXYFlip")) as GameObject;
			}
			//right
			else if(above < thresh && left < thresh && right >= thresh && below < thresh  && topLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "squeeze")) as GameObject;
			}	
			else if(above < thresh && left < thresh && right >= thresh && below < thresh  && bottomLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "squeezeYFlip")) as GameObject;
			}	
			
			//narrow entries
			else if( left < thresh && right < thresh && below < thresh && above < thresh && bottomLeft < thresh && bottomRight < thresh && topLeft >= thresh && topRight >=thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowEntry")) as GameObject;
			}
			else if( left < thresh && right < thresh && below < thresh && above < thresh && bottomLeft >= thresh && bottomRight >= thresh && topLeft < thresh && topRight < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowEntryYFlip")) as GameObject;
			}	
			else if( left < thresh && right < thresh && below < thresh && above < thresh && bottomLeft >= thresh && topLeft >= thresh && bottomRight < thresh && topRight < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowEntryXFlip90")) as GameObject;
			}			
			else if( left < thresh && right < thresh && below < thresh && above < thresh && topRight >= thresh && bottomRight >= thresh && topLeft < thresh && bottomLeft < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowEntry90")) as GameObject;
			}		
			
			//place fringe case puddles
			else if(below >= thresh && left >= thresh && right >= thresh && above < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowEnd")) as GameObject;
			}
			else if(above >= thresh && left >= thresh && right >= thresh && below < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowEndYFlip")) as GameObject;
			}					
			else if(below >= thresh && above >= thresh && left >= thresh && right < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowEnd90")) as GameObject;
			}					
			else if(below >= thresh && above >= thresh && right >= thresh && left < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowEndXFlip90")) as GameObject;
			}

			//place fringe narrows
			else if(left < thresh && right < thresh && above >= thresh && below >= thresh ){
				tile = Instantiate (Resources.Load (rootFolder + "narrow90")) as GameObject;
			}
			else if(above < thresh && below < thresh && left >= thresh && right >= thresh ){
				tile = Instantiate (Resources.Load (rootFolder + "narrow")) as GameObject;
			}
			
			//place fringe case sides
			else if(above < thresh && below < thresh && right < thresh && topRight >= thresh && bottomRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "left")) as GameObject;
			}
			else if(above < thresh && below < thresh && left < thresh && topLeft >= thresh && bottomLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "right")) as GameObject;
			}
			else if(left < thresh && right < thresh && below < thresh && bottomLeft >= thresh && bottomRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "bottom")) as GameObject;
			}
			else if(left < thresh && right < thresh && above < thresh && topLeft >= thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "top")) as GameObject;
			}
			
			//narrow turns
			else if(above < thresh && right < thresh && left >= thresh && below >= thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowTurn")) as GameObject;
			}
			
			else if(below < thresh && right < thresh && left >= thresh && above >= thresh && bottomRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowTurnXFlip")) as GameObject;
			}
			else if(below < thresh && left < thresh && right >= thresh && above >= thresh && bottomLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowTurnXFlip90")) as GameObject;
			}
			else if(above < thresh && left < thresh && right >= thresh && below >= thresh && topLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "narrowTurnXYFlip90")) as GameObject;
			}
			
						//outer corners
			else if(right < thresh && above < thresh && left >= thresh && below >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "bottomRight")) as GameObject;
			}	
			else if(left < thresh && above < thresh && right >= thresh && below >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "bottomLeft")) as GameObject;
			}
			else if(right < thresh && below < thresh && left >= thresh && above >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "topRight")) as GameObject;
			}	
			else if(left < thresh && below < thresh && right >= thresh && above >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "topLeft")) as GameObject;
			}
			
			//top and bottom
			else if(above < thresh && below >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "bottom")) as GameObject;
			}
			else if(below < thresh && above >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "top")) as GameObject;
			}
			//right and left
			else if(right < thresh && left >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "right")) as GameObject;
			}					
			else if(left < thresh && right >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "left")) as GameObject;
			}
			
			//place double corners
			else if(topRight >= thresh && bottomLeft >= thresh && left < thresh && right < thresh && above < thresh && below < thresh && topLeft < thresh && bottomRight < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "doubleCorner90")) as GameObject;
			}
			else if(topLeft >= thresh && bottomRight >= thresh && left < thresh && right < thresh && above < thresh && below < thresh && topRight < thresh && bottomLeft < thresh){
				tile = Instantiate (Resources.Load (rootFolder + "doubleCorner")) as GameObject;
			}
			//inner corners
			//inner bottom left corner
			else if(left < thresh && above < thresh && topLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "innerBottomLeft")) as GameObject;
			}			
			//inner bottom right corner
			else if(right < thresh && above < thresh && topRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "innerBottomRight")) as GameObject;
			}	
			//inner top left corner
			else if(left < thresh && above < thresh && bottomLeft >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "innerTopLeft")) as GameObject;
			}			
			//inner top right corner
			else if(right < thresh && above < thresh && bottomRight >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "innerTopRight")) as GameObject;
			}	
			//place solo tile
			else if(below >= thresh && above >= thresh && right >= thresh && left >= thresh){
				tile = Instantiate (Resources.Load (rootFolder + "big")) as GameObject;
			}
			//center
			else{
				tile = Instantiate (Resources.Load (rootFolder + "center")) as GameObject;
			}
			
			tile.transform.parent = transform;
			//tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f + height, 0);
			//tile.GetComponent<SpriteRenderer> ().sortingOrder = tileOrder;
			Tile tileObject = tile.GetComponent<Tile> ();
			tileObject.posn = new Vector2(x,y);
			tileObject.offsetPosn = new Vector2(offsetX,offsetY);
			//tileObject.sortingOrder = tileOrder;
			//tileObject.height = height;
			//tileObject.height2 = height2;
			//tileObject.height3 = height3;
			tileObject.origin = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, 0);
			//tileObject.waterLevel = thresh;
			tiles.Add (tileObject);
			
			
			float h = f  ((offsetX - x) / 1f, (offsetY + y) / 1f, randZ) * 1f;
			tileObject.geoHeight = center;//h;
			//Color c = new Color (.7f + h*5f, .7f + h*5f,1f, 1f);
			Color c = new Color (1,1,1,1);
			tileObject.color = c;//
			//tileObject.GetComponent<SpriteRenderer> ().color = c;
			tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, tile.transform.position.z);

		} 	
	}
}
