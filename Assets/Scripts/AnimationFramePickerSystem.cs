using UnityEngine;
using System.Collections;

public class AnimationFramePickerSystem : MonoBehaviour {
	public string sheetname;
	private Sprite[] sprites;
	private SpriteRenderer sr;
	private string[] names;
	
	void Start () 
	{
	 sprites = Resources.LoadAll<Sprite>(sheetname); 
	 sr = GetComponent<SpriteRenderer> ();
	 names = new string[sprites.Length];
	
	 for(int i = 0; i < names.Length; i++) 
	 {
	     names[i] = sprites[i].name;
	 }
	}
	
	void ChangeSprite( int index )
	{
	 //Sprite sprite = sprites[index]
	 //sr.sprite = sprite;
	}
	
	void ChangeSpriteByName( string name )
	{
	 //Sprite sprite = sprites[Array.IndexOf(names, name)];
	 //sr.sprite = sprite;
	}
}