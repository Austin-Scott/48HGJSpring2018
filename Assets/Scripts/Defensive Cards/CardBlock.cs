using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBlock : Card {

	public override IEnumerator Use() {
        Shield.CreateShield(5, holder);
        //TODO: Draw new card
        //TODO: If this shield breaks this turn destroy this card
		return null;
	}
}
