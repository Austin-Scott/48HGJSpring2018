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

	/// target
	Character target;

	/// holder
	Character holder;

	/// removes the last card in the deck and returns it.
	public Card Draw() {
		int lastCardIndex = cards.Count - 1;
		if (lastCardIndex < 0) {
			Card newCard = GameController.CreateCard(typeof(CardPunch));
			cards.Add(newCard);
			Initialize(holder, target, board, player);
			return Draw();
		}
		Card card = cards[lastCardIndex];
		cards.RemoveAt(lastCardIndex);
		card.inDeck = false;
		return card;
	}

	// Constructors
	public Deck(List<Card> cards) {
		this.cards = cards;
	}
	public Deck(Card card) {
		cards = new List<Card>();
		cards.Add(card);
	}
	public Deck() {
		cards = new List<Card>();
	}

	/// Returns true if the deck contains a card of type.
    public bool containsCard(System.Type card)
    {
        foreach(Card c in cards)
        {
            if (c.GetType() == card) return true;
        }
        return false;
    }

	/// Positions the deck in it corresponding deck position.
	public IEnumerator PositionDeck() {
		Coroutine[] movementCoroutines = new Coroutine[cards.Count];
		Transform deckLocation;
		if (player) {
			deckLocation = board.playerDeckPosition;
		} else {
			deckLocation = board.enemyDeckPosition;
		}
		Vector3 deckPosition = deckLocation.position;

        //For each card in this deck, stack them into their character's deck
		for (int i = 0; i < cards.Count; i++) {
			movementCoroutines[i] = GameController.ControllerCoroutine(cards[i].SmoothTransform(deckPosition, deckLocation.rotation));
			deckPosition.y += 0.3f;
		}
		// Wait for all cards to be positioned before exiting coroutine.
		foreach (Coroutine coroutine in movementCoroutines) {
			yield return coroutine;
		}

	}

	/// Sets the cards friendy and target properties.
	public void Initialize(Character holder, Character target, Board board, bool player) {
		this.holder = holder;
		this.target = target;
		this.board = board;
		this.player = player;
		foreach (Card card in cards) {
			card.Initialize(holder, target);
		}
	}

	/// Adds a card to the deck.
	public void AddCard(Card card) {
		cards.Add(card);
	}

	/// Destroys all cards in the deck. Called when a character dies.
	public void DestroyAllCards() {
		foreach (Card card in cards) {
			card.ForceDestroy();
		}
	}

	/// Adds quantity of new instances of cards to the deck of type.
	public void AddNewCards(System.Type cardType, int quantity) {
		for (int i = 0; i < quantity; i++) {
			AddCard(GameController.CreateCard(cardType));
		}
	}

	/// Shuffles the deck by randomly switching cards 100 times.
	public void Shuffle() {
		for (int i = 0; i < 100; i++) {
			int randomIndex0 = Random.Range(0, cards.Count);
			int randomIndex1 = Random.Range(0, cards.Count);
			Card copy = cards[randomIndex0];
			cards[randomIndex0] = cards[randomIndex1];
			cards[randomIndex1] = copy;
		}
	}
}
