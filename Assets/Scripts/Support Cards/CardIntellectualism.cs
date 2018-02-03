using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIntellectualism : Card {

	public override IEnumerator Use() {
        //TODO: Draw two cards
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
