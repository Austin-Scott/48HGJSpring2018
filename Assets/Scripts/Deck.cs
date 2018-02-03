using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

	/// Cards in the deck
	List<Card> cards;

	public Card Draw() {
		int lastCardIndex = cards.Count - 1;
		if (lastCardIndex < 0) {
			return null; //TODO return a new instance of a card
		}
		Card card = cards[lastCardIndex];
		cards.RemoveAt(lastCardIndex);
		return card;
	}
}
