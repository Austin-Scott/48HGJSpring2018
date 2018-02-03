using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	/// True when a turn is commecning. If false, the player is in the planning phase, where he can move cards around on the board.
	public bool running { 
		get {
			if (currentPhase == 0) {
				return false;
			} else {
				return true;
			}
		}
	}

	public int currentPhase { get; private set; }

	/// Dictionary relating the characters with the cards they own.
	Dictionary<Character, List<Card>[]> cardsOnBoard = new Dictionary<Character, List<Card>[]>();

	/// Player
	Character player;

	/// Enemy
	Character enemy;

	public IEnumerator Initialize(Character player, Character enemy) {
		currentPhase = 0;
		this.player = player;
		this.enemy = enemy;
		yield return StartCoroutine(player.Initialize(15, 0, 0, new Deck(GameController.CreateCard(typeof(CardSlash))), true));
		yield return StartCoroutine(enemy.Initialize(20, 1, 1,  new Deck(GameController.CreateCard(typeof(CardSlash))), false));
		cardsOnBoard.Add(player, new List<Card>[3]);
		cardsOnBoard.Add(enemy, new List<Card>[3]);
		for (int i = 0; i < 3; i++) {
			cardsOnBoard[player][i] = new List<Card>();
			cardsOnBoard[enemy][i] = new List<Card>();
		}
		StartCoroutine(player.DrawCard());
	}

	/// Gets a card by global index. 0-2 is the players cards while 3-5 represents opponents cards.
	public Card GetCard(int phaseIndex, int cardIndex) {
		Character boardSideOwner;
		if (phaseIndex > 2) {
			boardSideOwner = enemy;
			phaseIndex -= 3;
		} else {
			boardSideOwner = player;
		}
		return GetCard(boardSideOwner, phaseIndex, cardIndex);
	}

	/// Gets a card by local index, relative to the character that owns the side of the board.
	public Card GetCard(Character boardSideOwner, int phaseindex, int cardIndex) {
		return cardsOnBoard[boardSideOwner][phaseindex][cardIndex];
	}

	/// Sets a card by global index. 0-2 is the players cards while 3-5 represents opponents cards.
	public bool SetCard(Card card, int phaseIndex, int cardIndex) {
		Character boardSideOwner;
		if (phaseIndex > 2) {
			boardSideOwner = enemy;
			phaseIndex -= 3;
		} else {
			boardSideOwner = player;
		}
		return SetCard(card, boardSideOwner, cardIndex);
	}

	/// Sets a card by local index, relative to the character that owns the side of the board.
	public bool SetCard(Card card, Character boardSideOwner, int index) {
		cardsOnBoard[boardSideOwner][index] = card;
		return true;
	}

	/// Searches all phases of the board for a card on board side owner.
	public bool HasCard(System.Type cardType, Character boardSideOwner) {
		for (int i = 0; i <= 2; i++) {
			if (!PhaseHasOccured(i + 1)) {
				continue;
			}
			if (GetCard(boardSideOwner, i).GetType() == cardType) {
				return true;
			}
		}
		return false;
	}

	/// Searches all past phases of the board for both players. I don't think any card needs this atm.
	// public bool HasCard(System.Type cardType) {
	// 	for (int i = 0; i <= 5; i++) {
	// 		if (GetCard(i).GetType() == cardType) {
	// 			return true;
	// 		}
	// 	}
	// 	return false;
	// }

	public bool PhaseHasOccured(int phase) {
		if (currentPhase > phase) {
			return true;
		} else {
			return false;
		}
	}
}
