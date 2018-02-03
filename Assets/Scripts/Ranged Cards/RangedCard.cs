using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedCard : Card {

	public int CalculateDamage(int baseDamage) {
		return baseDamage + holder.GetDexterity();
	}

	static System.Action UpdateRangedCardDamageText;

	public abstract void UpdateDamageText();

	public override void Initialize(Character holder, Character target) {
		base.Initialize(holder, target);
		UpdateRangedCardDamageText += UpdateDamageText;
		UpdateDamageText();
	}
}
