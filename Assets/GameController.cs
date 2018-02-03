using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	/// Array of all cards in the game.
	Card[] cards;

	/// Load resources
	void Awake() {
		//Resources.load all cards
		Shield.SetShieldPrefab(Resources.Load("Shield", typeof(Shield)) as Shield);
	}

	/// Initialize the game
	void Start() {

	}
}
