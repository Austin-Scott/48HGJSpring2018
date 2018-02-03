using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	/// Cards currently on the board for the opponent
	Card[] enemyPhaseCards = new Card[3];

	/// Cards currently on the board for the player
	Card[] playerPhaseCards = new Card[3];

	/// Current deck of the opponent
	Deck enemyDeck;

	/// current deck of the player
	Deck playerDeck;

	/// Player
	Character player;

	/// Enemy
	Character enemy;

	void Start() {
		
	}
}
