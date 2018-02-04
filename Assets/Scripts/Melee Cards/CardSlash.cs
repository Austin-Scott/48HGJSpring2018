using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlash : MeleeCard {

	public override IEnumerator Use() {
		if (target.getTotalShield() == 0) {
			target.Damage(CalculateDamage(3));
		} else {
			target.Damage(CalculateDamage(1));
		}
		yield break;
	}

	public override void UpdateDamageText() {
		text.text = "Deal (" + CalculateDamage(3) + ") damage if no shield is present.\nOtherwise deals 1 damage to shield.";
	}
}
