﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck {

	/// Cards in the deck
	List<Card> cards;

	/// removes the last card in the deck and returns it.
	public Card Draw() {
		int lastCardIndex = cards.Count - 1;
		if (lastCardIndex < 0) {
			return null; //TODO return a new instance of a card
		}
		Card card = cards[lastCardIndex];
		cards.RemoveAt(lastCardIndex);
		return card;
	}

	public Deck(List<Card> cards) {
		this.cards = cards;
	}
	public Deck(Card card) {
		cards = new List<Card>();
		cards.Add(card);
	}
}
