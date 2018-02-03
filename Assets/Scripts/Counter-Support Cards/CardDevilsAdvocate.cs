using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDevilsAdvocate : Card {

	public override IEnumerator Use() {
        //TODO: Destroy all unplayable cards in your hand
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
