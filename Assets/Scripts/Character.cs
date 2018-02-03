using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DamageType {
	melee,
	ranged
};

public class Character : MonoBehaviour {

	bool player;

	/// The ID of the last character created.
	public static int lastUsedCharacterID = -1;

	/// The ID of the character
	public int characterID { get; private set; }

	/// Health of the character. Duel is over when a character reaches 0 health.
	int health = 15;
    int maxHealth;
    public void IncreaseHealth(int amount)
    {
        if (amount > 0) {
            if (health + amount < maxHealth)
            {
                health += amount;
            } else
            {
                health = maxHealth;
            }
        } else
        {
            if (health + amount > 0)
            {
                health += amount;
            } else
            {
                health = 0;
            }
        }

	/// The deck of the character
	Deck deck;

	/// The board the character is on
	Board board;

	/// Strength of the character. Increases melee attack damage by this value.
	int strength = 0;
	public int GetStrength() { return strength; }
    public void IncreaseStrength(int amount) { strength += amount; }

	/// Dextyerity of the character. Increase magic 
	int dexterity = 0;
	public int GetDexterity() { return dexterity; }
    public void IncreaseDexterity(int amount) { dexterity += amount; }

	/// Max number of cards in a players hand.
	int maxHandSize = 8;

	/// Cards in the player's hand.
	List<Card> hand = new List<Card>();

	/// The shields the player has active.
	List<Shield> shields;

	/// Called when a character's shield is broken.
	public void BreakShield(Shield shield) {
		shields.Remove(shield);
	}

	public int getTotalShield() {
		int shieldTotal = 0;
		foreach (Shield shield in shields) {
			shieldTotal += shield.GetValue();
		}
		return shieldTotal;
	}
	
	/// Generic damage function. Called whenever a card deals damage.
	public bool Damage(int damage) {
		if (getTotalShield() > 0) {
			while (damage != 0 && shields.Count != 0) {
				damage = shields[0].Damage(damage);
			}
		}
		if (damage > 0) {
			return false;
		}
		return DamageToHealth(damage);
	}

	/// Called only when the character has no shields up.
	bool DamageToHealth(int damage) {
		if (board.HasCard(typeof(CardAdrenalineRush))) {
			
		}
		health -= damage;
		if (health <= 0) {
			return true;
		}
		return true;
	}

	/// Initializes a character at the beginning of a match.
	public IEnumerator Initialize(int health, int strength, int dexterity, Deck deck, bool player) {
		this.health = health;
        this.maxHealth = health;
		this.strength = strength;
		this.dexterity = dexterity;
		this.deck = deck;
		this.player = player;
		lastUsedCharacterID ++;
		characterID = lastUsedCharacterID;
		Vector3 deckPosition;
		if (player) {
			deckPosition = new Vector3(10f, 0f, -5f);
		} else {
			deckPosition = new Vector3(10f, 0f, 5f);
		}
		yield return StartCoroutine(deck.PositionDeck(deckPosition));
	}

	/// A bunch of functions so we can compare characters together. Important for the board/card dictionary.
    public override int GetHashCode() {
        return characterID;
    }
    public override bool Equals(object obj) {
        return Equals(obj as Character);
    }
    public bool Equals(Character obj) {
        return obj != null && obj.characterID == this.characterID;
    }

	/// Discards a random card from the characters hand.
	public IEnumerator DiscardRandom() {
		yield return StartCoroutine(Discard(Random.Range(0, hand.Count)));
	}

	/// Discards a specific card from players hand.
	public IEnumerator Discard(int index) {
		hand.RemoveAt(index);
		StartCoroutine(PositionHand());
		//TODO discard animation
		return null;
	}

	/// Add a card to the characters hand.
	public IEnumerator AddCard(Card card) {
		if (hand.Count >= maxHandSize) {
			yield break;
		}
		hand.Add(card);
		//TODO add card animation
		StartCoroutine(PositionHand());
	}

	/// Draws a card from the characters deck and adds it to the characters hand.
	public IEnumerator DrawCard() {
		Card card = deck.Draw();
		/// TODO draw animation
		yield return StartCoroutine(AddCard(card));
	}

	/// Repositions the hand. Should be called everytime a card is removed or added.
	public IEnumerator PositionHand() {
		Vector3 handLocation = Vector3.zero;
		int handCount = hand.Count;
		int halfHandCount = handCount / 2;
		float cardSeperation = 10f / handCount;
		Coroutine[] movementCoroutines = new Coroutine[handCount];
		// if odd number of cards.
		if (handCount % 2 == 1) {
			// move middle card to middle of hand
			movementCoroutines[halfHandCount] = StartCoroutine(hand[halfHandCount].LerpPosition(handLocation));
			// fan cards left of the middle card
			for (int i = halfHandCount - 1; i >= 0; i--) {
				movementCoroutines[i] = StartCoroutine(hand[i].LerpPosition(handLocation - new Vector3(cardSeperation*(halfHandCount-i), 0f, 0f)));
				//TODO rotate the cards to look natural
			}
			// fan cards right of the middle card
			for (int i = halfHandCount + 1; i < handCount; i++) {
				movementCoroutines[i] = StartCoroutine(hand[i].LerpPosition(handLocation + new Vector3(cardSeperation*(i - halfHandCount), 0f, 0f)));
				//TODO rotate the cards to look natural
			}
		}
		// if even number of cards
		else {
			// fan cards left of the middle
			for (int i = halfHandCount - 1; i >= 0; i--) {
				movementCoroutines[i] = StartCoroutine(hand[i].LerpPosition(handLocation - new Vector3(cardSeperation*(halfHandCount-i), 0f, 0f)));
				//TODO rotate the cards to look natural
			}
			// fan cards right of the middle
			for (int i = halfHandCount; i < handCount; i++) {
				movementCoroutines[i] = StartCoroutine(hand[i].LerpPosition(handLocation + new Vector3(cardSeperation*(i - halfHandCount), 0f, 0f)));
				//TODO rotate the cards to look natural
			}
		}

		// wait for all cards to finish moving before exiting routine
		foreach (Coroutine coroutine in movementCoroutines) {
			yield return coroutine;
		}
	}

	/// Places a card onto the board during plannning phase.
	public IEnumerator PlaceCard(Card card, int phaseIndex) {
		if (board.running) {
			yield break;
		}
		board.SetCard(card, this, phaseIndex);
		hand.Remove(card);
		//TODO place card animation
		StartCoroutine(PositionHand());
	}

	public IEnumerator RemoveCardFromBoard(int phaseIndex) {
		if (board.running) {
			yield break;
		}
		Card card = board.GetCard(this, phaseIndex);
		board.SetCard(null, this, phaseIndex);
		//TODO remove card animation
		yield return StartCoroutine(AddCard(card));
	}
}
