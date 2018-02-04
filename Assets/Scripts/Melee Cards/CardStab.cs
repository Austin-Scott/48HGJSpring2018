using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStab : MeleeCard {

	public override IEnumerator Use() {
                target.Damage(CalculateDamage(5));
                if (target.getTotalShield() > 0)
                {
                        DestroyAtEndOfTurn();
                }
                yield break;
	}

	public override void UpdateDamageText() {
                text.text = "Deal (" + CalculateDamage(5) + ") damage, if blocked,\nthis card is destroyed.";
	}
}
