using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStealth : Card {

	public override IEnumerator Use() {
        //TODO: Your opponent cannot attack next turn
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
