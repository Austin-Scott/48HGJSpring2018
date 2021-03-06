﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controls elements of the board
public class Board : MonoBehaviour {

	// events
	public static System.Action endTurn;
	public static System.Action startTurn;
	public static System.Action endPhase;

	/// Player's cards destroyed this game.
	public static List<Card> destroyedCards = new List<Card>();

	[SerializeField]
	public Transform playerDeckPosition;
	[SerializeField]
	public Transform enemyDeckPosition;
	[SerializeField]
	public Transform playerDiscardPosition;
	[SerializeField]
	public Transform enemyDiscardPosition;
	[SerializeField]
	public Transform playerHandPosition;
	[SerializeField]
	public Transform enemyHandPosition;
	// [SerializeField]
	// Transform viewDrawnCardPosition; //Could be implemented if you want the card to be shown to the player before putting it into hand.
	[SerializeField]
	public PhaseSlot[] phasePositions;

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

	/// Current phase of the game. 0 if planning. 1-3 for action phases.
	public int currentPhase { get; private set; }

	/// Dictionary relating the characters with the cards they own.
	Dictionary<Character, List<Card>[]> cardsOnBoard = new Dictionary<Character, List<Card>[]>();

	/// Player
	public static Character player;

	/// Enemy
	public Character enemy;

	/// Initializes the board. Called everytime a new character is introduced.
	public IEnumerator Initialize(Character player, Character enemy) {
		endTurn = null;
		startTurn = null;
		endPhase = null;
		currentPhase = 1;
		Board.player = player;
		this.enemy = enemy;
		SetPhaseCollider(false);

		Deck playerDeck;
		if (GameController.currentEnemyIndex == 0) {
			playerDeck = new Deck(GameController.CreateCard(typeof(CardSlash)));
			playerDeck.AddCard(GameController.CreateCard(typeof(CardSlash)));
			// playerDeck.AddCard(GameController.CreateCard(typeof(CardPistolShot)));
			// playerDeck.AddCard(GameController.CreateCard(typeof(CardRest)));
			// playerDeck.AddCard(GameController.CreateCard(typeof(CardFeign)));
			// playerDeck.AddCard(GameController.CreateCard(typeof(CardIntimidate)));
		} else {
			playerDeck = player.deck;
			// TODO return cards to player deck
		}

        Deck enemyDeck = enemy.CreateEnemyDeck(GameController.currentEnemyIndex);

		// Deck enemyDeck = new Deck(GameController.CreateCard(typeof(CardSlash)));
		// playerDeck.AddCard(GameController.CreateCard(typeof(CardSlash)));

        //Initializes both the player and the enemy and stacks their cards into their corresponding deck
		yield return StartCoroutine(player.Initialize(15, 0, 0, playerDeck, true, enemy, this));
		yield return StartCoroutine(enemy.Initialize(15, 0, 0,  enemyDeck, false, player, this));

		cardsOnBoard.Add(player, new List<Card>[3]);
		cardsOnBoard.Add(enemy, new List<Card>[3]);
		for (int i = 0; i < 3; i++) {
			cardsOnBoard[player][i] = new List<Card>();
			cardsOnBoard[enemy][i] = new List<Card>();
		}
		yield return StartCoroutine(enemy.DrawCard());
		yield return StartCoroutine(player.DrawCard());
		yield return StartCoroutine(enemy.DrawCard());
		yield return StartCoroutine(player.DrawCard());
		yield return StartCoroutine(enemy.DrawCard());
		yield return StartCoroutine(player.DrawCard());
		yield return StartCoroutine(enemy.DrawCard());
		yield return StartCoroutine(player.DrawCard());
		currentPhase = 0;
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
		// 0 cost cards can be placed anywhere.
		if (card.GetCost() != 0) {
			// Check if any previous cards are using the phase the card is trying to be placed in.
			foreach (Card previousCards in cardsOnBoard[boardSideOwner][phaseIndex]) {
				if (previousCards.GetCost() > 0) {
					return false;
				}
			}
			if (phaseIndex == 1) {
				foreach (Card previousCards in cardsOnBoard[boardSideOwner][0]) {
					if (previousCards.GetCost() > 1) {
						return false;
					}
				}
			} else if (phaseIndex == 2) {
				foreach (Card previousCards in cardsOnBoard[boardSideOwner][0]) {
					if (previousCards.GetCost() > 2) {
						return false;
					}
				}
				foreach (Card previousCards in cardsOnBoard[boardSideOwner][1]) {
					if (previousCards.GetCost() > 1) {
						return false;
					}
				}
			}
		}
		/// Check if any future cards are being planned if cost > 1
		if (card.GetCost() > 1) {
			if (phaseIndex == 2) {
				return false;
			}
			foreach (Card futureCards in cardsOnBoard[boardSideOwner][phaseIndex+1]) {
				if (futureCards.GetCost() > 0) {
					return false;
				}
			}
			if (card.GetCost() > 2) {
				if (phaseIndex != 0) {
					return false;
				}
				foreach (Card futureCards in cardsOnBoard[boardSideOwner][1]) {
					if (futureCards.GetCost() > 0) {
						return false;
					}
				}
				foreach (Card futureCards in cardsOnBoard[boardSideOwner][2]) {
					if (futureCards.GetCost() > 0) {
						return false;
					}
				}
			}
		}
		card.onBoard = true;
		cardsOnBoard[boardSideOwner][phaseIndex].Add(card);
		card.phaseIndex = phaseIndex;
		// convert relative phase index to global phase index.
		if (!boardSideOwner.player) {
			card.phaseIndex += 3;
		}
		return true;
	}

	/// Gets number of cards in a certain phase index.
	public int GetCardCount(Character boardSideOwner, int phaseIndex) {
		// convert to relative phase index if fed global one.
		if (phaseIndex > 2) {
			phaseIndex -= 3;
		}
		return cardsOnBoard[boardSideOwner][phaseIndex].Count;
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
		// card.onBoard = false;
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

	/// Checks if a phase has already occurred or not.
	public bool PhaseHasOccured(int phase) {
		if (currentPhase > phase) {
			return true;
		} else {
			return false;
		}
	}

	/// Sets up the game board for the next turn's planning phase
	IEnumerator NextTurn() {
		// return all cards that where not destroyed to the player's hand.
		for (int i = 0; i < 3; i++) {
			foreach (Card card in cardsOnBoard[player][i]) {
				if (card != null) {
					yield return StartCoroutine(player.AddCard(card));
				}
			}
			for (int j = cardsOnBoard[player][i].Count-1; j >= 0; j--) {
				cardsOnBoard[player][i].RemoveAt(j);
			}
			foreach (Card card in cardsOnBoard[enemy][i]) {
				if (card != null) {
					yield return StartCoroutine(enemy.AddCard(card));
				}
			}
			for (int j = cardsOnBoard[enemy][i].Count-1; j >= 0; j--) {
				cardsOnBoard[enemy][i].RemoveAt(j);
			}
		}
		if (startTurn != null) {
			startTurn(); //might want to be at beginning of commence phase, idk
		}
		yield return StartCoroutine(enemy.DrawCard());
		yield return StartCoroutine(player.DrawCard());
		currentPhase = 0;
	}

	/// Ends the planning phase and starts to use the cards.
	public IEnumerator Commence() {
		yield return StartCoroutine(enemy.PlayAuto());
		for (int i = 0; i < 3; i++) {
			yield return CommencePhase(currentPhase);
			if (endPhase != null) {
				endPhase();
			}
		}
		if (endTurn != null) {
			endTurn();
		}
		StartCoroutine(NextTurn());
	}

	/// Runs a single phase.
	public IEnumerator CommencePhase(int phaseIndex) {
		currentPhase++;
		List<Card> playerCards = cardsOnBoard[player][phaseIndex];
		List<Card> enemyCards = cardsOnBoard[enemy][phaseIndex];
		List<Card> attackCards = new List<Card>();
		List<Card> nonAttackCards = new List<Card>();

		// add all cards to their respective list
		foreach (Card card in playerCards) {
			if (card == null) {
				Debug.LogError("Null card in player hand");
			}
			if (card as MeleeCard == null && card as RangedCard == null) {
				nonAttackCards.Add(card);
			} else {
				attackCards.Add(card);
			}
		}
		foreach (Card card in enemyCards) {
			if (card == null) {
				Debug.LogError("Null card in player hand");
			}
			if (card as MeleeCard == null && card as RangedCard == null) {
				nonAttackCards.Add(card);
			} else {
				attackCards.Add(card);
			}
		}
		// append nonattack cards with attack cards.
		nonAttackCards.AddRange(attackCards);
		// Use the cards, defensive ones first, attack ones next.
		foreach (Card card in nonAttackCards) {
			if (card == null) { Debug.Log(card.holder.player); }
			yield return StartCoroutine(card.Use());
		}
	}

	/// enter key commences
	void Update() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			FightButtonPressed();
		}
	}

	/// Called when the fight button is pressed or enter key is pressed.
	public void FightButtonPressed() {
		if (!running) {
			StartCoroutine(Commence());
		}
	}

	/// Enables or disables all phases. Called when the player is dragging a card.
	public void SetPhaseCollider (bool active) {
		foreach (PhaseSlot phaseSlot in phasePositions) {
			phaseSlot.gameObject.SetActive(active);
		}
	}

	/// Setup board for next enemy.
	public IEnumerator NextEnemy() {
		GameController.currentEnemyIndex++;
		for (int i = 0; i < 3; i++) {
			foreach (Card card in cardsOnBoard[player][i]) {
				Destroy(card.gameObject);
			}
			cardsOnBoard[player][i] = new List<Card>();
			foreach (Card card in cardsOnBoard[enemy][i]) {
				Destroy(card.gameObject);
			}
			cardsOnBoard[enemy][i] = new List<Card>();
		}
		enemy.DestroyAllCards();
		Destroy(enemy.gameObject);
		enemy = GameController.CreateEnemy();
		player.ShuffleBackDeck(destroyedCards);
		//TODO yield return textbox
		yield return StartCoroutine(Initialize(player, enemy));
	}

	/// Called only when the game is restarted
	public void DeleteAll() {
		enemy.DestroyAllCards();
		player.DestroyAllCards();
		Destroy(enemy.gameObject);
		Destroy(player.gameObject);
		Destroy(gameObject);
	}
}
