using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	/// Character this shield is attached to.
	Character character;

	/// ID of the shield
	int shieldID;
	static int lastUsedShieldID = -1;

	/// Amount of damage the shield can absorb before it is destroyed.
	int value;

	/// Returns the number of damage the shield can block.
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
		lastUsedShieldID++;
		newShield.shieldID = lastUsedShieldID;
		return newShield;
	}

	/// Deals damage to the shield, and returns remaining damage.
	public int Damage(int damage) {
		if (damage >= value) {
			value -= damage;
			character.BreakShield(this);
			return value * -1;
		} else {
			value -= damage;
			return 0;
		}
	}

	/// A bunch of functions so we can compare shields together.
    public override int GetHashCode() {
        return shieldID;
    }
    public override bool Equals(object obj) {
        return Equals(obj as Shield);
    }
    public bool Equals(Shield obj) {
        return obj != null && obj.shieldID == this.shieldID;
    }
}
