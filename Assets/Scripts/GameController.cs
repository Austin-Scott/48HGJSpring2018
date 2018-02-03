using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	/// Array of all cards in the game.
	Card[] cards;
	/// Prefab for a board
	Board boardPrefab;
	/// Prefab for a character
	Character characterPrefab;

	Board currentBoard;

	/// Load resources
	void Awake() {
		gameController = this;
		//Resources.load all cards
		Shield.SetShieldPrefab(Resources.Load("Shield", typeof(Shield)) as Shield);
	}

	/// Initialize the game
	void Start() {
		currentBoard = Instantiate(boardPrefab);
		currentBoard.Initialize(Instantiate(characterPrefab), Instantiate(characterPrefab));
	}

	/// Creates a card of type
	public static Card CreateCard(System.Type cardType) {
		if (cardType == typeof(CardSlash)) {
			return Instantiate(gameController.cards[0]) as CardSlash;
		} else {
			Debug.LogError("Card does not exits");
			return null;
		}
	}

	static GameController gameController;
}
