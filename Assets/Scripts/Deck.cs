using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck {

	/// Cards in the deck
	List<Card> cards;

	/// the board the deck is on
	Board board;

	// /// target
	// DeckHolder target;

	/// holder
	DeckHolder holder;

	/// removes the last card in the deck and returns it.
	public Card Draw() {
		int lastCardIndex = cards.Count - 1;
		// if (lastCardIndex < 0) {
		// 	Card newCard = GameController.CreateCard(typeof(CardPunch));
		// 	cards.Add(newCard);
		// 	Initialize(holder, target, board, player);
		// 	return Draw();
		// }
		Card card = cards[lastCardIndex];
		cards.RemoveAt(lastCardIndex);
		// card.inDeck = false;
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

	/// Sets the cards friendy and target properties.
	public void Initialize(DeckHolder holder, Board board) {
		this.holder = holder;
		this.board = board;
		foreach (Card card in cards) {
			card.Initialize(holder);
		}
	}

	/// Adds a card to the deck.
	public void AddCard(Card card) {
		cards.Add(card);
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
