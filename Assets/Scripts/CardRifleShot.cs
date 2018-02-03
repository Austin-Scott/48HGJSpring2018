using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRifleShot : RangedCard {

	public override IEnumerator Use() {
        //TODO: make damage to be of type ranged
        target.Damage(5);
        //TODO: add reload to holder's hand
		return null;
	}
}
