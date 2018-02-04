using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeCard : Card {

	public static System.Action UpdateMeleeCardDamageText;

	public int CalculateDamage(int damage) {
		return damage + holder.GetStrength();
	}

	public abstract void UpdateDamageText();

	public override void Initialize(Character holder, Character target) {
		base.Initialize(holder, target);
		UpdateMeleeCardDamageText += UpdateDamageText;
		UpdateDamageText();
	}
}
