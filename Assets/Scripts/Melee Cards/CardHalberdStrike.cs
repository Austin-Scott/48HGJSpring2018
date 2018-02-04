using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHalberdStrike : MeleeCard {

	public override IEnumerator Use() {
        target.DamageToHealth(15);
		return null;
	}

	public override void UpdateDamageText() {
        text.text = "Deal (15) damage to health.\nCannot be blocked.";
	}
}
