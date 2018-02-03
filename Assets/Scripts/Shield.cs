using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	/// Character this shield is attached to.
	Character character;

	/// Amount of damage the shield can absorb before it is destroyed.
	int shieldAmount;


	/// Prefab of shield
	static Shield shieldPrefab;
	public static void SetShieldPrefab(Shield prefab) { Shield.shieldPrefab = prefab; }

	public static Shield CreateShield(int shieldAmount, Character character) {
		Shield newShield = Instantiate(shieldPrefab);
		newShield.shieldAmount = shieldAmount;
		newShield.character = character;
		return newShield;
	}
}
