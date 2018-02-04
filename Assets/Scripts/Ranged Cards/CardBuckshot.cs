using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBuckshot : RangedCard {

	public override IEnumerator Use() {
        target.Damage(CalculateDamage(1));
        target.Damage(CalculateDamage(1));
        target.Damage(CalculateDamage(1));

        DestroyAtEndOfTurn();
        yield break;
	}

	public override void UpdateDamageText() {
        text.text = "Deal (" + CalculateDamage(1) + ") ranged damage 3 times. Adds a reload to your hand.";
	}
}
