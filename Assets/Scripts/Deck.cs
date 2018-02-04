using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck {

	/// Cards in the deck
	List<Card> cards;

	/// the board the deck is on
	Board board;

	/// True if this deck belongs to the player
	bool player;

	/// removes the last card in the deck and returns it.
	public Card Draw() {
		int lastCardIndex = cards.Count - 1;
		if (lastCardIndex < 0) {
			return null; //TODO return a new instance of a card
		}
		Card card = cards[lastCardIndex];
		cards.RemoveAt(lastCardIndex);
		card.inDeck = false;
		return card;
	}

	public Deck(List<Card> cards) {
		this.cards = cards;
	}
	public Deck(Card card) {
		cards = new List<Card>();
		cards.Add(card);
	}

	public IEnumerator PositionDeck(Vector3 position) {
		Coroutine[] movementCoroutines = new Coroutine[cards.Count];
		Transform deckLocation;
		if (player) {
			deckLocation = board.playerDeckPosition;
		} else {
			deckLocation = board.enemyDeckPosition;
		}
		for (int i = 0; i < cards.Count; i++) {
			movementCoroutines[i] = GameController.ControllerCoroutine(cards[i].SmoothTransform(deckLocation));
			Vector3 deckPosition = deckLocation.position;
			deckPosition.y += 0.3f;
			deckLocation.position = deckPosition;
		}
		foreach (Coroutine coroutine in movementCoroutines) {
			yield return coroutine;
		}
	}

	/// Sets the cards firendy and target properties.
	public void Initialize(Character holder, Character target, Board board, bool player) {
		this.board = board;
		this.player = player;
		foreach (Card card in cards) {
			card.Initialize(holder, target);
		}
	}
}
