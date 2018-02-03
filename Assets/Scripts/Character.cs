using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DamageType {
	melee,
	ranged
};

public class Character : MonoBehaviour {

	/// Health of the character. Duel is over when a character reaches 0 health.
	int health = 15;

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

	public int getTotalShield() {
		int shieldTotal = 0;
		foreach (Shield shield in shields) {
			shieldTotal += shield.GetValue();
		}
		return shieldTotal;
	}
	
	public bool Damage(int damage) {
		health -= damage;
		if (health <= 0) {
			return true;
		}
		return true;
	}

	public void DiscardRandom() {

	}

	public void Discard(int index) {

	}

	public bool AddCard(Card card) {

	}
}
