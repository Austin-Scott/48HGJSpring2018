using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeCard : Card {

	static System.Action UpdateMeleeCardDamageText;

	public int CalculateDamage(int damage) {
		return damage + holder.GetStrength();
	}

	public abstract void UpdateDamageText();

	protected override void Awake() {
		UpdateMeleeCardDamageText += UpdateDamageText;
		UpdateDamageText();
	}
}
