using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeCard : Card {

	public int CalculateDamage(int damage) {
		return damage + holder.GetStrength();
	}
}
