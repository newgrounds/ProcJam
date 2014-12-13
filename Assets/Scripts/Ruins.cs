using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ruins
{
	public List<Decal> wallsTopAndLeft = new List<Decal> ();
	public List<Decal> wallsRight = new List<Decal> ();
	public List<Decal> wallsBottom = new List<Decal> ();
	public List<Decal> floors = new List<Decal> ();
	public GameObject roof;
	private float flatHeight;
	private float flatY;
	private List<Tile> tiles;
	public enum FLOOR
	{
		redwood,
		lightwood,
		darkwood,
		stone
	};

	public enum DECAL
	{
		crate,
		jar,
		barrel,
		chest
	};
	private int recursiveCount;
	TerrainChunk chunk;
	
	public Ruins (int mapWidth, int startIndex, TerrainChunk c, int recursiveCount)
	{
		this.recursiveCount = recursiveCount;
		this.roof =  GameObject.Instantiate (Resources.Load ("ruins/EmptyRoof")) as GameObject;
		this.roof.transform.parent = c.transform;
		// store passed in chunk
		chunk = c;
		
		// choose random floor type
		FLOOR floorType = (FLOOR)Random.Range (0, 3);
		tiles = chunk.tiles;
		//int randStartPos = new Vector2(Random.Range(0,tiles.Count),Random.Range(0,tiles.Count));
		int randStartPos = startIndex;
		int randWidth = Random.Range (2, 4);
		int randHeight = Random.Range (2, 4);
		
		//place corners
		CreateWallDecal (randStartPos, "ruins/ruins_topLeft");
		CreateWallDecal (randStartPos + randHeight, "ruins/ruins_bottomLeft");
		CreateWallDecal (randStartPos + (mapWidth * randWidth), "ruins/ruins_topRight");
		CreateWallDecal (randStartPos + randHeight + (mapWidth * randWidth), "ruins/ruins_bottomRight");
		
		if (randStartPos < tiles.Count) {
			Tile startTile = tiles [randStartPos];
			flatHeight = startTile.height;
			flatY = startTile.transform.position.y;
		
			// place top walls
			for (int i = 1; i < randWidth; i++) {
				wallsTopAndLeft.AddRange (CreateWallDecal (randStartPos + (mapWidth * i), "ruins/ruins_top"));
			}
		
			// place bottom walls
			for (int i = 1; i < randWidth; i++) {
				wallsBottom.AddRange (CreateWallDecal (randStartPos + randHeight + (mapWidth * i), "ruins/ruins_top"));
			}
		
			// place right walls
			for (int i = 1; i < randHeight; i++) {
				wallsRight.AddRange (CreateWallDecal (randStartPos + (mapWidth * randWidth) + i, "ruins/ruins_side"));
			}
			
			// place left walls
			for (int i = 1; i < randHeight; i++) {
				wallsTopAndLeft.AddRange (CreateWallDecal (randStartPos + i, "ruins/ruins_side"));
			}
			
			// create floors
			for (int x = 0; x <randWidth; x++) {
				for (int y = 0; y <randHeight; y++) {
					CreateFloorDecal (randStartPos + (mapWidth * x) + y, "ruins/" + floorType.ToString ());
				}
			}
			
			// remove wall to create door
			RemoveRandomWall (ref wallsTopAndLeft);
		
			if (recursiveCount < 3) {
				// chance to spawn another ruin to the right
				if (Random.Range (0, 100) > 80) {
					RemoveRandomWall (ref wallsRight);
					chunk.ruins.Add (new Ruins (mapWidth, startIndex + (randWidth * mapWidth), chunk, recursiveCount++));
				} else {
					RemoveRandomWall (ref wallsRight);	
				}
				// chance to spawn another ruin below
				if (Random.Range (0, 100) > 80) {
					RemoveRandomWall (ref wallsBottom);
					chunk.ruins.Add (new Ruins (mapWidth, startIndex + randHeight, chunk, recursiveCount++));
				} else {
					RemoveRandomWall (ref wallsBottom);	
				}
			}

			
		} else {
			Debug.Log ("failed to create ruins");
		}
	}
	
	void RemoveRandomWall (ref List<Decal> wallList)
	{
		if (wallList.Count > 0) {
			Decal wall = wallList [Random.Range (0, wallList.Count)];
			CreateDoorDecal (wall.tile, "ruins/ruins_door");
			wallList.Remove (wall);
			GameObject.DestroyImmediate (wall.gameObject);
		}
	}

	List<Decal> CreateWallDecal (int index, string wallType)
	{
		List<Decal> tempWalls = new List<Decal> ();
		if (index < tiles.Count) {
			Tile t = tiles [index];
			// this is stupid C# madness
			GameObject wall = null;
			//if(Random.Range(0,10) > 2){
			if (t.GetDecal () == null) {
				t.transform.position = t.origin;
				//}
				wall = GameObject.Instantiate (Resources.Load (wallType)) as GameObject;
				wall.transform.parent = chunk.transform;
				wall.transform.position = new Vector3 (t.transform.position.x - .2f, t.transform.position.y + 0.2f, -2);
				wall.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 2;
				t.SetDecal (wall.GetComponent<Decal> ());
				//t.height = flatHeight;
				tempWalls.Add (wall.GetComponent<Decal> ());
			} else if (t.GetDecal ().CompareTag ("Wall")) {
				wall = GameObject.Instantiate (Resources.Load (wallType)) as GameObject;
				wall.transform.parent = chunk.transform;
				wall.transform.position = new Vector3 (t.transform.position.x - .2f, t.transform.position.y + 0.2f, -2);
				wall.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 2;
				t.SetDecal (wall.GetComponent<Decal> ());
				tempWalls.Add (wall.GetComponent<Decal> ());
			}
		}
		return tempWalls;
	}
	
	void CreateFloorDecal (int index, string wallType)
	{
		if (index < tiles.Count) {
			Tile t = tiles [index];
			//if(Random.Range(0,10) > 2){
			t.transform.position = t.origin;
			//}
			GameObject wall = GameObject.Instantiate (Resources.Load (wallType)) as GameObject;
			GameObject roof = GameObject.Instantiate (Resources.Load ("ruins/roofTile")) as GameObject;
			wall.transform.parent = chunk.transform;
			roof.transform.parent = this.roof.transform;
			roof.transform.position = new Vector3 (t.transform.position.x + .05f, t.transform.position.y + .15f, 0);
			
			if (Random.Range (0, 10) > 7) {
				DECAL decalType = (DECAL)Random.Range (0, 4);
				GameObject decal = GameObject.Instantiate (Resources.Load (decalType.ToString ())) as GameObject;
				decal.transform.parent = chunk.transform;
				decal.transform.position = new Vector3 (t.transform.position.x /*+ Random.Range(-.1f,.1f)*/, t.transform.position.y /*+ Random.Range(-.1f,.1f)*/, -3);
				decal.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 2;	
			}
			
			wall.transform.position = new Vector3 (t.transform.position.x + .05f, t.transform.position.y - .225f, 0);
			//wall.GetComponent<SpriteRenderer>().sortingOrder = t.sortingOrder + 1;
			t.SetDecal (wall.GetComponent<Decal> ());
			t.height = flatHeight;
			floors.Add (wall.GetComponent<Decal> ());
		}
	}
	
	void CreateDoorDecal (Tile t, string wallType)
	{
		t.transform.position = t.origin;
		GameObject door = GameObject.Instantiate (Resources.Load (wallType)) as GameObject;
		door.transform.parent = chunk.transform;
		door.transform.position = new Vector3 (t.transform.position.x + .05f, t.transform.position.y + .05f, 0);
		door.GetComponent<SpriteRenderer> ().sortingOrder = t.sortingOrder + 2;
		t.SetDecal (door.GetComponent<Decal> ());
	}
}
