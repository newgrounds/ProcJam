using UnityEngine;
using System.Collections.Generic;

public class Cryptogram : MonoBehaviour {
	public GameObject book;
	public GameObject chest;
	public int numChests = 5;
	public string message;
	
	Dictionary<char, char> dict = new Dictionary<char, char>() {
		{'a',' '},{'b',' '},{'c',' '},{'d',' '},{'e',' '},{'f',' '},{'g',' '},{'h',' '},{'i',' '},
		{'j',' '},{'k',' '},{'l',' '},{'m',' '},{'n',' '},{'o',' '},{'p',' '},{'q',' '},{'r',' '},
		{'s',' '},{'t',' '},{'u',' '},{'v',' '},{'w',' '},{'x',' '},{'y',' '},{'z',' '}
	};
	
	List<char> letters = new List<char>() {
		'a','b','c','d','e','f','g','h','i',
		'j','k','l','m','n','o','p','q','r',
		's','t','u','v','w','x','y','z'
	};
	
	void Start() {
		GenerateDictionary();
		Debug.Log(dict);
		
		PlaceBooks();
		
		PlaceChests();
	}
	
	void GenerateDictionary() {
		List<char> keys = new List<char>(dict.Keys);
		foreach (char letter in keys) {
			char mappedLetter = letters[Random.Range(0, letters.Count)];
			dict[letter] = mappedLetter;
			Debug.Log(letter + " = " + mappedLetter);
			letters.Remove(mappedLetter);
		}
	}
	
	/*
	 * Scrambles a word with the generated dictionary
	 */
	public string Scramble(string original) {
		string scrambled = "";
		
		// loop over the original word
		foreach (char letter in original.ToLower()) {
			// replace letter if we have a mapping
			if (dict.ContainsKey(letter)) {
				scrambled += dict[letter];
			} else {
				scrambled += letter;
			}
		}
		Debug.Log(scrambled);
		return scrambled;
	}
	
	public void UnscrambleValue(char unscrambled) {
		// replace letter if we have a mapping
		if (dict.ContainsKey(unscrambled)) {
			dict[unscrambled] = unscrambled;
		}
	}
	
	public void PlaceBooks() {
		foreach (char letter in dict.Keys) {
			Vector3 pos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
			GameObject bObj = (GameObject)Instantiate(book, pos, Quaternion.identity);
			Book b = bObj.GetComponent<Book>();
			b.bookChar = letter;
		}
	}
	
	public void PlaceChests() {
		List<char> keys = new List<char>(dict.Keys);
		for (int i = 0; i < numChests; i++) {
			Vector3 pos = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
			GameObject cObj = (GameObject)Instantiate(chest, pos, Quaternion.identity);
			Chest c = cObj.GetComponent<Chest>();
			c.storedChar = keys[Random.Range(0, keys.Count)];
			message += c.storedChar;
			keys.Remove(c.storedChar);
		}
		Scramble(message);
	}
	
	void OnGUI() {
		GUI.Box(new Rect(25, 15, 275, 75), "");
		for (int i = 0; i < message.Length; i++) {
			char m = message[i];
			Rect pos = new Rect(50 + (50 * i), 30, 50, 30);
			
			// if the character has been deciphered
			if (dict[m].Equals(m)) {
				// draw it as green
				GUI.color = Color.green;
			} else {
				// draw it as red
				GUI.color = Color.red;
			}
			
			// draw the char
			GUI.Label(pos, "<size=40>" + Scramble(m+"") + "</size>");
		}
	}
}