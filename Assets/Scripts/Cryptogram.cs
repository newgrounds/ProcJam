using UnityEngine;
using System.Collections.Generic;

public class Cryptogram : MonoBehaviour {
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
		string message = Scramble("This is scrambled");
		UnscrambleValue('t');
		Scramble("This is scrambled");
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
}