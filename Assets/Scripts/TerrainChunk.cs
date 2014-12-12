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
		StartCoroutine(GenerateChunk());
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
	
	IEnumerator GenerateChunk() {
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
				float height2 = SimplexNoise.Noise.Generate ((offsetX - x) / 8f, (offsetY + y) / 8f, randZ-1) / 2f;
				float height3 = SimplexNoise.Noise.Generate ((offsetX - x) / 15f, (offsetY + y) / 15f, randZ) / 2f;
				float height4 = SimplexNoise.Noise.Generate ((offsetX - x) / 8f, (offsetY + y) / 8f, randZ-2) / 2f;
				int tileOrder = (y + (int)offsetY) + (10 * (int)offsetY);
				
				//string waterFolder = "Water/";
	
				CreateTileLayer(x,y, .1f, -2f, "Water/", TerrainGenerator.GetWaterNoise, -2);
				CreateTileLayer(x,y, .175f, -2f, "Sand/", TerrainGenerator.GetSandNoise, -1);
				CreateTileLayer(x,y, -.3f, -2f, "Dirt/", TerrainGenerator.GetDirtNoise, -3);
				CreateTileLayer(x,y, .5f, -2f, "Desert/", TerrainGenerator.GetDesertNoise, -4);
				//CreateTileLayer(x,y, .5f, -2f, "Stone/", TerrainGenerator.GetStoneNoise, -5);
				
				//CreateTileLayer(x,y, .1f, "Water/");
				float h = TerrainGenerator.GetWaterNoise  ((offsetX - x) / 1f, (offsetY + y) / 1f, randZ) * 1f;
				if (h >= -2f) {
					GameObject tile;
					tile = Instantiate (Resources.Load ("tile"), new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, 0), Quaternion.identity) as GameObject;
					tile.transform.parent = transform;
					
					tile.GetComponent<SpriteRenderer> ().sortingOrder = tileOrder;
					Tile tileObject = tile.GetComponent<Tile> ();
					tileObject.posn = new Vector2(x,y);
					tileObject.offsetPosn = new Vector2(offsetX,offsetY);
					tileObject.sortingOrder = tileOrder;
					tileObject.height = height;
					tileObject.height2 = height2;
					tileObject.height3 = height3;
					tileObject.height4 = height4;
					tileObject.origin = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, 0);
					
					tiles.Add (tileObject);
					
					tileObject.geoHeight = h;
					//Color c = new Color ((200f/255f) + h, 1f - h/10f, 0, 1);
					Color c = new Color((200f/255f),1,0,1);
					tileObject.color = c;
					tileObject.GetComponent<SpriteRenderer> ().color = c;
				}
			}
			yield return null;
		}
		
		// generate bunnies
		if (Random.Range(0,10) > 5){
			Vector3 bunnyPos = tiles[Random.Range(0,tiles.Count)].transform.position;
			GameObject bunny = Instantiate(Resources.Load("Animals/MisterBunny"), bunnyPos, Quaternion.identity) as GameObject;
			bunny.transform.parent = transform;
		}
		
		// generate wolves
		if (Random.Range(0,10) > 7){
			Vector3 wolfPos = tiles[Random.Range(0,tiles.Count)].transform.position;
			GameObject wolf = Instantiate(Resources.Load("Animals/BigBadWolf"), wolfPos, Quaternion.identity) as GameObject;
			wolf.transform.parent = transform;
		}
		
		// generate ruins
		/*for (int i = 0; i < ruinsToSpawn; i++) {
			ruins.Add (new Ruins (mapWidth, Random.Range (0, 0), this, 0));
		}
		
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
			float height4 = t.height4;
			float geoHeight = t.geoHeight;
			if (t.GetDecal () == null) {
				if (geoHeight > thresh && height2 > .3f) {
					float randomSize = Random.Range (-.5f, .5f);
					float xOffset = 0;//Random.Range(-.5f, .5f) ;
					
					GameObject tree;
					
					if (height3 > 0f) {
						tree = Instantiate(Resources.Load("tempTree")) as GameObject;
					} else {
						tree = Instantiate(Resources.Load("pine")) as GameObject;
					}
					
					tree.transform.parent = transform;
					tree.transform.localScale = new Vector3(1f + randomSize, 1f + randomSize, 1f);
					tree.transform.position = new Vector3(t.transform.position.x, t.transform.position.y, -2);
					tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer>().sortingOrder = t.sortingOrder + 2;	
					
					tree.GetComponent<Decal>().child.GetComponent<SpriteRenderer>().color = new Color(1 + Random.Range(-.25f, 0), 1 + Random.Range(-.25f, 0), Random.Range(0f, .3f), 1);	
					t.SetDecal(tree.GetComponent<Decal>());
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
				}*/
			}
			
			// spawn coins
			if (t.GetDecal() == null) {
				if (height3 > .1f && geoHeight > thresh) {
					string type = "Coin";
					//if(Random.Range(0,10) > 9) type = "gem"; else type = "Coin";
					GameObject coin = Instantiate (Resources.Load(type), new Vector3(t.transform.position.x, t.transform.position.y, 0), Quaternion.identity) as GameObject;
					coin.transform.parent = transform;
					//tree.transform.localScale = new Vector3 (1f + randomSize, 1f + randomSize, 1f);
					coin.GetComponent<SpriteRenderer>().sortingOrder = t.sortingOrder + 3;
					//tree.GetComponent<SpriteRenderer> ().color = new Color (1 + Random.Range (-.25f, 0), 1 + Random.Range (-.25f, 0), Random.Range (0f, .3f), 1);	
					//t.SetDecal (coin.GetComponent<Decal> ());
				} 
			}
			
			// spawn food
			if (t.GetDecal() == null) {
				if (height4 > 0.1f && geoHeight > thresh) {
					if (Random.Range(0, 10) > 9) {
						if (height3 > 0.4f) {
							GameObject food = Instantiate(Resources.Load("food/apple"), new Vector3(t.transform.position.x, t.transform.position.y, 0), Quaternion.identity) as GameObject;
						} else if (height3 > 0.2f) {
							GameObject food = Instantiate(Resources.Load("food/cherry"), new Vector3(t.transform.position.x, t.transform.position.y, 0), Quaternion.identity) as GameObject;
						} else if (height3 > 0) {
							GameObject food = Instantiate(Resources.Load("food/egg"), new Vector3(t.transform.position.x, t.transform.position.y, 0), Quaternion.identity) as GameObject;
						} else if (height3 > -0.2f) {
							GameObject food = Instantiate(Resources.Load("food/cheese"), new Vector3(t.transform.position.x, t.transform.position.y, 0), Quaternion.identity) as GameObject;
						} else if (height3 > -0.4f) {
							GameObject food = Instantiate(Resources.Load("food/bread"), new Vector3(t.transform.position.x, t.transform.position.y, 0), Quaternion.identity) as GameObject;
						}
					}
				}
			}
			
			// get a list of tiles without decals
			if (t.GetDecal() == null) {
				tilesWithoutDecals.Add(t);
			}
			
		}
		
		yield return null;
	}
	
	void PrintValidTile(string type, float val, float thresh){
		Debug.Log(type + ": " + val + " / " + thresh + ", " + (val < thresh));		
	}
	
	void CreateTileLayer(int x, int y, float thresh, float minThresh, string rootFolder, TerrainGenerator.GetGlobalNoise f, int z){

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
		
		string assetPath;

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
				assetPath = rootFolder + "4Split";
			}
			
			//4split with gap
			else if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft >= thresh && bottomRight >= thresh
				&& topLeft >= thresh && topRight < thresh){
				assetPath = rootFolder + "4SplitGapXYFlip";
			}	
			else if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft >= thresh && bottomRight >= thresh
				&& topLeft < thresh && topRight >= thresh){
				assetPath = rootFolder + "4SplitGapYFlip";
			}	
			else if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft < thresh && bottomRight >= thresh
				&& topLeft >= thresh && topRight >= thresh){
				assetPath = rootFolder + "4SplitGap";
			}	
			else if(above < thresh && right < thresh && left < thresh && below < thresh 
				&& bottomLeft >= thresh && bottomRight < thresh
				&& topLeft >= thresh && topRight >= thresh){
				assetPath = rootFolder + "4SplitGapXFlip";
			}	
			//tsplit
			//left/right
			else if(above < thresh && left >= thresh && right < thresh && below < thresh && bottomRight >= thresh && topRight >= thresh){
				assetPath = rootFolder + "tsplitXFlip";
			}
			else if(above < thresh && right >= thresh && left < thresh && below < thresh && bottomLeft >= thresh && topLeft >= thresh){
				assetPath = rootFolder + "tsplit";
			}	
			//top/bottom
			else if(above < thresh && right < thresh && left < thresh && below >= thresh && topLeft >= thresh && topRight >= thresh){
				assetPath = rootFolder + "tsplit90";
			}	
			else if(above >= thresh && right < thresh && left < thresh && below < thresh && bottomLeft >= thresh && bottomRight >= thresh){
				assetPath = rootFolder + "tsplitYFlip90";
			}			
			
			//squeezes
			//above
			else if(above >= thresh && left < thresh && right < thresh && below < thresh && bottomRight >= thresh){
				assetPath = rootFolder + "squeezeXFlip90";
			}
			else if(above >= thresh && left < thresh && right < thresh && below < thresh && bottomLeft >= thresh){
				assetPath = rootFolder + "squeezeXYFlip90";
			}
			//below
			else if(above < thresh && left < thresh && right < thresh && below >= thresh && topLeft >= thresh){
				assetPath = rootFolder + "squeezeYFlip90";
			}
			else if(above < thresh && left < thresh && right < thresh && below >= thresh && topRight >= thresh){
				assetPath = rootFolder + "squeeze90";
			}
			
			
			//left
			else if(above < thresh && left >= thresh && right < thresh && below < thresh && topRight >= thresh){
				assetPath = rootFolder + "squeezeXFlip";
			}
			else if(above < thresh && left >= thresh && right < thresh && below < thresh && bottomRight >= thresh){
				assetPath = rootFolder + "squeezeXYFlip";
			}
			//right
			else if(above < thresh && left < thresh && right >= thresh && below < thresh  && topLeft >= thresh){
				assetPath = rootFolder + "squeeze";
			}	
			else if(above < thresh && left < thresh && right >= thresh && below < thresh  && bottomLeft >= thresh){
				assetPath = rootFolder + "squeezeYFlip";
			}	
			
			//narrow entries
			else if( left < thresh && right < thresh && below < thresh && above < thresh && bottomLeft < thresh && bottomRight < thresh && topLeft >= thresh && topRight >=thresh){
				assetPath = rootFolder + "narrowEntry";
			}
			else if( left < thresh && right < thresh && below < thresh && above < thresh && bottomLeft >= thresh && bottomRight >= thresh && topLeft < thresh && topRight < thresh){
				assetPath = rootFolder + "narrowEntryYFlip";
			}	
			else if( left < thresh && right < thresh && below < thresh && above < thresh && bottomLeft >= thresh && topLeft >= thresh && bottomRight < thresh && topRight < thresh){
				assetPath = rootFolder + "narrowEntryXFlip90";
			}			
			else if( left < thresh && right < thresh && below < thresh && above < thresh && topRight >= thresh && bottomRight >= thresh && topLeft < thresh && bottomLeft < thresh){
				assetPath = rootFolder + "narrowEntry90";
			}		
			
			//place fringe case puddles
			else if(below >= thresh && left >= thresh && right >= thresh && above < thresh){
				assetPath = rootFolder + "narrowEnd";
			}
			else if(above >= thresh && left >= thresh && right >= thresh && below < thresh){
				assetPath = rootFolder + "narrowEndYFlip";
			}					
			else if(below >= thresh && above >= thresh && left >= thresh && right < thresh){
				assetPath = rootFolder + "narrowEnd90";
			}					
			else if(below >= thresh && above >= thresh && right >= thresh && left < thresh){
				assetPath = rootFolder + "narrowEndXFlip90";
			}

			//place fringe narrows
			else if(left < thresh && right < thresh && above >= thresh && below >= thresh ){
				assetPath = rootFolder + "narrow90";
			}
			else if(above < thresh && below < thresh && left >= thresh && right >= thresh ){
				assetPath = rootFolder + "narrow";
			}
			
			//place fringe case sides
			else if(above < thresh && below < thresh && right < thresh && topRight >= thresh && bottomRight >= thresh){
				assetPath = rootFolder + "left";
			}
			else if(above < thresh && below < thresh && left < thresh && topLeft >= thresh && bottomLeft >= thresh){
				assetPath = rootFolder + "right";
			}
			else if(left < thresh && right < thresh && below < thresh && bottomLeft >= thresh && bottomRight >= thresh){
				assetPath = rootFolder + "bottom";
			}
			else if(left < thresh && right < thresh && above < thresh && topLeft >= thresh && topRight >= thresh){
				assetPath = rootFolder + "top";
			}
			
			//narrow turns
			else if(above < thresh && right < thresh && left >= thresh && below >= thresh && topRight >= thresh){
				assetPath = rootFolder + "narrowTurn";
			}
			
			else if(below < thresh && right < thresh && left >= thresh && above >= thresh && bottomRight >= thresh){
				assetPath = rootFolder + "narrowTurnXFlip";
			}
			else if(below < thresh && left < thresh && right >= thresh && above >= thresh && bottomLeft >= thresh){
				assetPath = rootFolder + "narrowTurnXFlip90";
			}
			else if(above < thresh && left < thresh && right >= thresh && below >= thresh && topLeft >= thresh){
				assetPath = rootFolder + "narrowTurnXYFlip90";
			}
			
						//outer corners
			else if(right < thresh && above < thresh && left >= thresh && below >= thresh){
				assetPath = rootFolder + "bottomRight";
			}	
			else if(left < thresh && above < thresh && right >= thresh && below >= thresh){
				assetPath = rootFolder + "bottomLeft";
			}
			else if(right < thresh && below < thresh && left >= thresh && above >= thresh){
				assetPath = rootFolder + "topRight";
			}	
			else if(left < thresh && below < thresh && right >= thresh && above >= thresh){
				assetPath = rootFolder + "topLeft";
			}
			
			//top and bottom
			else if(above < thresh && below >= thresh){
				assetPath = rootFolder + "bottom";
			}
			else if(below < thresh && above >= thresh){
				assetPath = rootFolder + "top";
			}
			//right and left
			else if(right < thresh && left >= thresh){
				assetPath = rootFolder + "right";
			}					
			else if(left < thresh && right >= thresh){
				assetPath = rootFolder + "left";
			}
			
			//place double corners
			else if(topRight >= thresh && bottomLeft >= thresh && left < thresh && right < thresh && above < thresh && below < thresh && topLeft < thresh && bottomRight < thresh){
				assetPath = rootFolder + "doubleCorner90";
			}
			else if(topLeft >= thresh && bottomRight >= thresh && left < thresh && right < thresh && above < thresh && below < thresh && topRight < thresh && bottomLeft < thresh){
				assetPath = rootFolder + "doubleCorner";
			}
			//inner corners
			//inner bottom left corner
			else if(left < thresh && above < thresh && topLeft >= thresh){
				assetPath = rootFolder + "innerBottomLeft";
			}			
			//inner bottom right corner
			else if(right < thresh && above < thresh && topRight >= thresh){
				assetPath = rootFolder + "innerBottomRight";
			}	
			//inner top left corner
			else if(left < thresh && above < thresh && bottomLeft >= thresh){
				assetPath = rootFolder + "innerTopLeft";
			}			
			//inner top right corner
			else if(right < thresh && above < thresh && bottomRight >= thresh){
				assetPath = rootFolder + "innerTopRight";
			}	
			//place solo tile
			else if(below >= thresh && above >= thresh && right >= thresh && left >= thresh){
				assetPath = rootFolder + "big";
				
			}
			//center
			else{
				assetPath = rootFolder + "center";
			}
			tile = Instantiate (Resources.Load (assetPath)) as GameObject;
			
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
			tileObject.color = c;
			
			
			//
			
			//tileObject.GetComponent<SpriteRenderer> ().color = c;
			tile.transform.position = new Vector3 (transform.position.x + x * .5f, transform.position.y - y * .5f, tile.transform.position.z);

		} 	
	}
}
