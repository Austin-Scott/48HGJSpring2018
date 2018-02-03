using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMakeShiftArmor : Card {

	public override IEnumerator Use() {
        //TODO: do not lose shields at the end of this turn
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
