using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIntimidate : Card {

	public override IEnumerator Use() {
        yield return target.DiscardRandom();
        DestroyAtEndOfTurn();
        yield break;
	}
}
