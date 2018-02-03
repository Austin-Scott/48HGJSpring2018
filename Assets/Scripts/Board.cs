using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	/// True when a turn is commecning. If false, the player is in the planning phase, where he can move cards around on the board.
	public bool running = false;

	/// Dictionary relating the characters with the cards they own.
	Dictionary<Character, Card[]> cardsOnBoard = new Dictionary<Character, Card[]>();

	/// Player
	Character player;

	/// Enemy
	Character enemy;

	public IEnumerator Initialize(Character player, Character enemy) {
		this.player = player;
		this.enemy = enemy;
		yield return StartCoroutine(player.Initialize(15, 0, 0, new Deck(GameController.CreateCard(typeof(CardSlash))), true));
		yield return StartCoroutine(enemy.Initialize(20, 1, 1,  new Deck(GameController.CreateCard(typeof(CardSlash))), false));
		cardsOnBoard.Add(player, new Card[3]);
		cardsOnBoard.Add(enemy, new Card[3]);
		StartCoroutine(player.DrawCard());
	}

	/// Gets a card by global index. 0-2 is the players cards while 3-5 represents opponents cards.
	public Card GetCard(int index) {
		Character boardSideOwner;
		if (index > 2) {
			boardSideOwner = enemy;
			index -= 3;
		} else {
			boardSideOwner = player;
		}
		return GetCard(boardSideOwner, index);
	}

	/// Gets a card by local index, relative to the character that owns the side of the board.
	public Card GetCard(Character boardSideOwner, int index) {
		return cardsOnBoard[boardSideOwner][index];
	}

	/// Sets a card by global index. 0-2 is the players cards while 3-5 represents opponents cards.
	public bool SetCard(Card card, int index) {
		Character boardSideOwner;
		if (index > 2) {
			boardSideOwner = enemy;
			index -= 3;
		} else {
			boardSideOwner = player;
		}
		return SetCard(card, boardSideOwner, index);
	}

	/// Sets a card by local index, relative to the character that owns the side of the board.
	public bool SetCard(Card card, Character boardSideOwner, int index) {
		cardsOnBoard[boardSideOwner][index] = card;
		return true;
	}
}
