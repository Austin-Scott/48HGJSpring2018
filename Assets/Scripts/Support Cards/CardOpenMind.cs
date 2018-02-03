using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOpenMind : Card {

	public override IEnumerator Use() {
        //TODO: increase your hand size by 2
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
