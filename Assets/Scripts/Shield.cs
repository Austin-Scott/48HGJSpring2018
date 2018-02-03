using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	/// Character this shield is attached to.
	Character character;

	/// Amount of damage the shield can absorb before it is destroyed.
	int value;

	public int GetValue() {
		return value;
	}


	/// Prefab of shield
	static Shield shieldPrefab;
	public static void SetShieldPrefab(Shield prefab) { Shield.shieldPrefab = prefab; }

	public static Shield CreateShield(int shieldAmount, Character character) {
		Shield newShield = Instantiate(shieldPrefab);
		newShield.value = shieldAmount;
		newShield.character = character;
		return newShield;
	}
}
