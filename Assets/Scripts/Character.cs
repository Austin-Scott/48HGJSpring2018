using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	/// Health of the character. Duel is over when a character reaches 0 health.
	int health = 15;

	/// Strength of the character. Increases melee attack damage by this value.
	int strength = 0;

	/// Dextyerity of the character. Increase magic 
	int dexterity = 0;

	/// The shields the player has active.
	List<Shield> shields;

	
	bool Damage(int damage) {
		health -= damage;
		if (health <= 0) {
			return true;
		}
		return true;
	}
}
