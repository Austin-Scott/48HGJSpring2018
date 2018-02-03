using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLightningBolt : RangedCard {

	public override IEnumerator Use() {
        //TODO: make damage to be of type ranged
        target.Damage(7);
        //TODO: destroy this card
		return null;
	}
}
