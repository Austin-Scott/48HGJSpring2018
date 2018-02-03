using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public static System.Action endTurn;
	public static System.Action startTurn;
	public static System.Action endPhase;

	[SerializeField]
	Transform playerDeckPosition;
	[SerializeField]
	Transform enemyDeckPostion;
	[SerializeField]
	Transform playerDiscardPosition;
	[SerializeField]
	Transform enemyDiscardPosition;
	// [SerializeField]
	// Transform viewDrawnCardPosition;
	[SerializeField]
	Transform[] playerPhasePositions;
	[SerializeField]
	Transform[] enemyPhasePosition;

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
		endTurn = null;
		startTurn = null;
		endPhase = null;
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
	public bool AddCard(Card card, int phaseIndex) {
		Character boardSideOwner;
		if (phaseIndex > 2) {
			boardSideOwner = enemy;
			phaseIndex -= 3;
		} else {
			boardSideOwner = player;
		}
		return AddCard(card, boardSideOwner, phaseIndex);
	}

	/// Sets a card by local index, relative to the character that owns the side of the board.
	public bool AddCard(Card card, Character boardSideOwner, int phaseIndex) {
		cardsOnBoard[boardSideOwner][phaseIndex].Add(card);
		return true;
	}

	/// Removes a card by global index. 0-2 is the players cards while 3-5 represents opponents cards.
	public bool RemoveCard(Card card, int phaseIndex) {
		Character boardSideOwner;
		if (phaseIndex > 2) {
			boardSideOwner = enemy;
			phaseIndex -= 3;
		} else {
			boardSideOwner = player;
		}
		return RemoveCard(card, boardSideOwner, phaseIndex);
	}

	/// Removes a card by local index, relative to the character that owns the side of the board.
	public bool RemoveCard(Card card, Character boardSideOwner, int phaseIndex) {
		cardsOnBoard[boardSideOwner][phaseIndex].Remove(card);
		return true;
	}

	/// Searches all phases of the board for a card on board side owner.
	public bool HasCard(System.Type cardType, Character boardSideOwner) {
		for (int i = 0; i <= 2; i++) {
			if (!PhaseHasOccured(i + 1)) {
				continue;
			}
			foreach (Card card in cardsOnBoard[boardSideOwner][i]) {
				if (card.GetType() == cardType) {
					return true;
				}
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

	/// Sets up the game board for the next turn's planning phase
	IEnumerator NextTurn() {
		currentPhase = 0;
		// return all cards that where not destroyed to the player's hand.
		for (int i = 0; i < 3; i++) {
			foreach (Card card in cardsOnBoard[player][i]) {
				if (card != null) {
					yield return StartCoroutine(player.AddCard(card));
				}
			}
			foreach (Card card in cardsOnBoard[enemy][i]) {
				if (card != null) {
					yield return StartCoroutine(enemy.AddCard(card));
				}
			}
		}
		startTurn();
	}

	/// Ends the planning phase and starts to use the cards.
	public IEnumerator Commence() {
		for (int i = 0; i < 3; i++) {
			yield return CommencePhase(currentPhase);
			endPhase();
		}
		endTurn();
		StartCoroutine(NextTurn());
	}

	public IEnumerator CommencePhase(int phaseIndex) {
		currentPhase++;
		List<Card> playerCards = cardsOnBoard[player][phaseIndex];
		List<Card> enemyCards = cardsOnBoard[enemy][phaseIndex];
		List<Card> attackCards = new List<Card>();
		List<Card> nonAttackCards = new List<Card>();

		// add all cards to their respective list
		foreach (Card card in playerCards) {
			if (card as MeleeCard == null && card as RangedCard == null) {
				nonAttackCards.Add(card);
			} else {
				attackCards.Add(card);
			}
		}
		foreach (Card card in enemyCards) {
			if (card as MeleeCard == null && card as RangedCard == null) {
				nonAttackCards.Add(card);
			} else {
				attackCards.Add(card);
			}
		}
		
		nonAttackCards.AddRange(attackCards);
		foreach (Card card in nonAttackCards) {
			yield return StartCoroutine(card.Use());
		}
	}
}
