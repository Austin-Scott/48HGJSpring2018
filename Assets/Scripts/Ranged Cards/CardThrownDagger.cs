using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardThrownDagger : RangedCard {

	public override IEnumerator Use() {
        target.Damage(CalculateDamage(3));
        DestroyAtEndOfTurn();
		return null;
	}

	public override void UpdateDamageText() {
        text.text = "Deals (" + CalculateDamage(3) + ") ranged damage. Card is destroyed.";
	}
}
