using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLightningBolt : RangedCard {

	public override IEnumerator Use() {
        target.Damage(CalculateDamage(7));
        DestroyAtEndOfTurn();
        return null;
	}

	public override void UpdateDamageText() {
        text.text = "Deal (" + CalculateDamage(7) + ") ranged damage. Card is destroyed.";
	}
}
