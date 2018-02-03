using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBuckshot : RangedCard {

	public override IEnumerator Use() {
        //TODO: make damage to be of type ranged
        target.Damage(1);
        target.Damage(1);
        target.Damage(1);
        target.Damage(1);
        //TODO: add reload to holder's hand
		return null;
	}

	public override void UpdateDamageText() {
		//TODO
	}
}
