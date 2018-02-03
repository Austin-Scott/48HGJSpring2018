using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArmor : Card {

	public override IEnumerator Use() {
        Shield.CreateShield(3, holder);
        //TODO: if shield breaks, destroy this card
		return null;
	}
}
