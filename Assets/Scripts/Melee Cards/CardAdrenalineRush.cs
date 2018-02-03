using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAdrenalineRush : Card {

	public override IEnumerator Use() {
        //TODO: Everytime you take damage to health this turn gain 2 strength
        DestroyAtEndOfTurn();
        yield break;
	}
}
