using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHypnosis : Card {

	public override IEnumerator Use() {
        //TODO: destroy the next card used by your opponent
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
