using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIntimidate : Card {

	public override IEnumerator Use() {
        //TODO: opponent discards a random card
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
