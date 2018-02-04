using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public bool player { get; private set; }

    /// The ID of the last character created.
    public static int lastUsedCharacterID = -1;

    /// The ID of the character
    public int characterID { get; private set; }

    /// Health of the character. Duel is over when a character reaches 0 health.
    int health = 15;

    /// Max health of the character.
    int maxHealth;

    /// The deck of the character
    public Deck deck;

    /// The board the character is on
    Board board;

    //If true this character can sell their soul instead of dieing
    public bool canSellSoul { get; private set; }
    //If true this character has sold their soul and should receive a Souless type card
    public bool soldSoul { get; private set; }

    /// Strength of the character. Increases melee attack damage by this value.
    int strength = 0;
	public int GetStrength() { return strength; }
    public void IncreaseStrength(int amount) { strength += amount; }

	/// Dextyerity of the character. Increase ranged attacks 
	int dexterity = 0;
	public int GetDexterity() { return dexterity; }
    public void IncreaseDexterity(int amount) { dexterity += amount; }

    /// Max number of cards in a players hand.
    int maxHandSize = 8;
    public void changeMaxHandSize(int amount)
    {
        maxHandSize += amount;
        if (maxHandSize < 1) maxHandSize -= amount;
    }

	/// Cards in the player's hand.
	List<Card> hand = new List<Card>();

	/// The shields the player has active.
	List<Shield> shields = new List<Shield>();

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

	/// Heal function. idk why its not called that.
    public void IncreaseHealth(int amount) {
		if (amount <= 0) {
			return;
		}
		health += amount;
		if (health > maxHealth) {
			health = maxHealth;
		}
	}

    public bool hasCard(System.Type card)
    {
        if (deck.containsCard(card)) return true;
        if (board.HasCard(card, this)) return true;
        return false;
    }
	
	/// Generic damage function. Called whenever a card deals damage.
	public bool Damage(int damage) {
		if (getTotalShield() > 0) {
			while (damage != 0 && shields.Count != 0) {
				damage = shields[0].Damage(damage);
			}
		}
		if (damage <= 0) {
			return false;
		}
		return DamageToHealth(damage);
	}

	/// Called only when the character has no shields up or is hit by an undodgable attack or damage breaks shields.
	public bool DamageToHealth(int damage) {
        //TODO: Interupt channel cards
		if (board.HasCard(typeof(CardAdrenalineRush), this)) {
			IncreaseStrength(2);
		}
		health -= damage;
		Debug.Log("Dealt " + damage + ". Character now has " + health + " health");
		if (health <= 0) {
            if (!canSellSoul) {
                return false;
            } else {
                canSellSoul = false;
                soldSoul = true;
                health = 1;
                return true;
            }
		}
		return true;
	}

	/// Initializes a character at the beginning of a match.
	public IEnumerator Initialize(int health, int strength, int dexterity, Deck deck, bool player, Character target, Board board) {
		this.health = health;
        this.maxHealth = health;
		this.strength = strength;
		this.dexterity = dexterity;
		this.deck = deck;
		this.board = board;
		deck.Initialize(this, target, board, player);
		this.player = player;
        this.canSellSoul = false;
        this.soldSoul = false;

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
			Debug.LogWarning("Bug here?");
			yield return card.Destroy();
			yield break;
		}
		hand.Add(card);
		//TODO add card animation
		yield return StartCoroutine(PositionHand());
		card.onBoard = false;
	}

	/// Draws a card from the characters deck and adds it to the characters hand.
	public IEnumerator DrawCard() {
		Card card = deck.Draw();
		/// TODO draw animation
		yield return StartCoroutine(AddCard(card));
	}

	/// Repositions the hand. Should be called everytime a card is removed or added.
	public IEnumerator PositionHand() {
		Transform handLocation;
		if (player) {
			handLocation = board.playerHandPosition;
		} else {
			handLocation = board.enemyHandPosition;
		}
		Vector3 originalLocation = handLocation.position;
		Quaternion originalRotation = handLocation.rotation;
		int handCount = hand.Count;
		int halfHandCount = handCount / 2;
		float cardSeperation = 2f;
		float rotationSeperation = 5f;
		Coroutine[] movementCoroutines = new Coroutine[handCount];
		// if odd number of cards.
		if (handCount % 2 == 1) {
			// move middle card to middle of hand
			Quaternion middleRotation = originalRotation;
			Vector3 middleHandLocation = originalLocation + new Vector3(0f, 0.2f*halfHandCount, 0f);
			movementCoroutines[halfHandCount] = StartCoroutine(hand[halfHandCount].SmoothTransform(middleHandLocation, middleRotation, 2f));
			// fan cards left of the middle card
			for (int i = halfHandCount - 1; i >= 0; i--) {
				Quaternion newRotation = originalRotation;
				Vector3 eulerAngles = newRotation.eulerAngles;
				eulerAngles.y += -rotationSeperation * (halfHandCount-i);
				newRotation.eulerAngles = eulerAngles;
				Vector3 newHandLocation = originalLocation + new Vector3(-cardSeperation*(halfHandCount-i), 0.2f*i, 0f);
				movementCoroutines[i] = StartCoroutine(hand[i].SmoothTransform(newHandLocation, newRotation, 2f));
			}
			// fan cards right of the middle card
			for (int i = halfHandCount + 1; i < handCount; i++) {
				Quaternion newRotation = originalRotation;
				Vector3 eulerAngles = newRotation.eulerAngles;
				eulerAngles.y += rotationSeperation * (i - halfHandCount);
				newRotation.eulerAngles = eulerAngles;
				Vector3 newHandLocation = originalLocation + new Vector3(cardSeperation*(i - halfHandCount), 0.2f*i, 0f);
				movementCoroutines[i] = StartCoroutine(hand[i].SmoothTransform(newHandLocation, newRotation, 2f));
			}
		}
		// if even number of cards
		else {
			// fan cards left of the middle
			for (int i = halfHandCount - 1; i >= 0; i--) {
				Quaternion newRotation = originalRotation;
				Vector3 eulerAngles = newRotation.eulerAngles;
				eulerAngles.y += -rotationSeperation * (halfHandCount-i);
				newRotation.eulerAngles = eulerAngles;
				Vector3 newHandLocation = originalLocation + new Vector3(-cardSeperation*(halfHandCount-i), 0.2f*i, 0f);
				movementCoroutines[i] = StartCoroutine(hand[i].SmoothTransform(newHandLocation, newRotation, 2f));
			}
			// fan cards right of the middle
			for (int i = halfHandCount; i < handCount; i++) {
				Quaternion newRotation = originalRotation;
				Vector3 eulerAngles = newRotation.eulerAngles;
				eulerAngles.y += rotationSeperation * (i - halfHandCount);
				newRotation.eulerAngles = eulerAngles;
				Vector3 newHandLocation = originalLocation + new Vector3(cardSeperation*(i - halfHandCount), 0.2f*i, 0f);
				movementCoroutines[i] = StartCoroutine(hand[i].SmoothTransform(newHandLocation, newRotation, 2f));
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
		if (!board.AddCard(card, this, phaseIndex)) {
			yield break;
		}
		hand.Remove(card);
		Coroutine moveCardRoutine = StartCoroutine(card.PositionOnBoard());
		Coroutine positionHandRoutine = StartCoroutine(PositionHand());
		yield return moveCardRoutine;
		// Debug.Log("card placed");
		yield return positionHandRoutine;
		// Debug.Log("hand arranged");
	}

	public IEnumerator RemoveCardFromBoard(Card card, int phaseIndex) {
		if (board.running) {
			yield break;
		}
		board.RemoveCard(card, this, phaseIndex);
		//TODO remove card animation
		int remaingCardCount = board.GetCardCount(this, phaseIndex);
		Coroutine[] positionCardRoutines = new Coroutine[remaingCardCount];
		Coroutine addCardRoutine = StartCoroutine(AddCard(card));
		for (int i = 0; i < remaingCardCount; i++) {
			positionCardRoutines[i] = StartCoroutine(board.GetCard(this, phaseIndex, i).PositionOnBoard());
		}
		yield return addCardRoutine;
		foreach (Coroutine coroutine in positionCardRoutines) {
			yield return coroutine;
		}
	}

	/// Returns true if hand is busu
	public bool GetHandBusy() {
		foreach (Card card in hand) {
			if (card.GetBusy()) {
				return true;
			}
		}
		return false;
	}

	public IEnumerator PlayAuto () {
		// List<Card>[] sortedCards = new List<Card>[4];
		// for (int i = 0; i < 4; i++) {
		// 	sortedCards[i] = new List<Card>();
		// }
		// foreach (Card card in hand) {
		// 	sortedCards[card.GetCost()].Add(card);
		// }
		List<Card> canPlay = new List<Card>(hand);
		List<Card> plannedToPlay = new List<Card>();
		int time = 3;
		while (time != 0) {
			for (int i = canPlay.Count-1; i >= 0; i--) {
				if (canPlay[i].GetCost() > time) {
					canPlay.RemoveAt(i);
				}
			}
			if (canPlay.Count == 0) {
				break;
			}
			// choose random card to play
			int randomCardIndex = Random.Range(0, canPlay.Count);
			plannedToPlay.Add(canPlay[randomCardIndex]);
			time -= canPlay[randomCardIndex].GetCost();
			canPlay.RemoveAt(randomCardIndex);
		}
		time = 0;
		foreach (Card card in plannedToPlay) {
			yield return PlaceCard(card, time);
			time += card.GetCost();
			if (time > 2) {
				yield break;
			}
		}
	}
}
