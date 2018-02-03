using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSleepingGas : Card {

	public override IEnumerator Use() {
        //TODO: both players discard their hand and draw 3 cards
        yield return StartCoroutine(this.Destroy());
        //TODO: end turn
        yield break;
	}
}
