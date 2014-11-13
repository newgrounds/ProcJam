using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ruins {
	
	private float flatHeight;
	private float flatY;
	private List<Tile> tiles;
	// Use this for initialization
	public Ruins (int mapWidth) {
		
		tiles = TerrainGenerator.tiles;
			//int randStartPos = new Vector2(Random.Range(0,tiles.Count),Random.Range(0,tiles.Count));
		int randStartPos = Random.Range(0, tiles.Count);
		int randWidth = Random.Range(2,6);
		int randHeight = Random.Range(2,6);
		
		//place corners
		CreateWallDecal(randStartPos, "ruins/ruins_topLeft");
		CreateWallDecal(randStartPos + randHeight, "ruins/ruins_bottomLeft");
		CreateWallDecal(randStartPos + (mapWidth * randWidth), "ruins/ruins_topRight");
		CreateWallDecal(randStartPos + randHeight + (mapWidth * randWidth), "ruins/ruins_bottomRight");
		
		Tile startTile = tiles[randStartPos];
		flatHeight = startTile.height;
		flatY = startTile.transform.position.y;
		
		//place wall spans
		for(int i = 1; i < randWidth; i++){
			CreateWallDecal(randStartPos + (mapWidth*i), "ruins/ruins_top");
		}

		for(int i = 1; i < randHeight; i++){
			CreateWallDecal(randStartPos + i, "ruins/ruins_side");
		}
		
		
		for(int i = 1; i < randWidth; i++){
			CreateWallDecal(randStartPos + randHeight + (mapWidth*i), "ruins/ruins_top");
		}
		
		
		for(int i = 1; i < randHeight; i++){
			CreateWallDecal(randStartPos + (mapWidth * randWidth) + i, "ruins/ruins_side");
		}
		
		for(int x = 0; x <randWidth; x++){
			for(int y = 0; y <randHeight; y++){
				CreateFloorDecal(randStartPos + (mapWidth * x) + y, "ruins/ruins_floor");
			}
		}
		
	}
	void CreateWallDecal(int index, string wallType){
		if(index < tiles.Count){
			Tile t = tiles[index];
			//if(Random.Range(0,10) > 2){
			t.transform.position = t.origin;
			//}
			GameObject wall = GameObject.Instantiate (Resources.Load (wallType)) as GameObject;
			wall.transform.position = new Vector3 (t.transform.position.x - .2f, t.transform.position.y + .7f, 0);
			wall.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 1;
			t.decal = wall;	
			//t.height = flatHeight;
		}
	}
	
	void CreateFloorDecal(int index, string wallType){
		if(index < tiles.Count){
			Tile t = tiles[index];
			//if(Random.Range(0,10) > 2){
			t.transform.position = t.origin;
			//}
			GameObject wall = GameObject.Instantiate (Resources.Load (wallType)) as GameObject;
			wall.transform.position = new Vector3 (t.transform.position.x + .05f, t.transform.position.y  + .7f, 0);
			wall.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 1;
			t.decal = wall;	
			t.height = flatHeight;
		}
	}
}
