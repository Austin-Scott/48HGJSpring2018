﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlash : MeleeCard {

	public override IEnumerator Use() {
		if (target.getTotalShield() == 0) {
			target.Damage(CalculateDamage(3));
		} else {
			target.Damage(CalculateDamage(1));
		}
		return null;
	}

	public override void UpdateDamageText() {
		text.text = "Deal (" + CalculateDamage(1) + ") damage. Deal 2 additional damage to health.";
	}
}