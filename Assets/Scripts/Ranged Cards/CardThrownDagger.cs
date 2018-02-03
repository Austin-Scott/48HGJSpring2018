using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardThrownDagger : RangedCard {

	public override IEnumerator Use() {
        //TODO: make damage to be of type ranged
        target.Damage(3);
        //TODO: Destroy this card
		return null;
	}

	public override void UpdateDamageText() {
		//TODO
	}
}
