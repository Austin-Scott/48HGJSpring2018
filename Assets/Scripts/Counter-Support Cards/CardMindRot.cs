using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMindRot : Card {

	public override IEnumerator Use() {
        //TODO: Choose a card in your opponents hand, destroy all copies of it
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
