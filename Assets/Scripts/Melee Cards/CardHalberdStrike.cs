using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHalberdStrike : MeleeCard {

	public override IEnumerator Use() {
        target.DamageToHealth(CalculateDamage(15));
		yield break;
	}

	public override void UpdateDamageText() {
        	text.text = "Deal (" + CalculateDamage(15) + ") damage to health.\nCannot be blocked.";
	}
}
