using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBandage : Card {

	public override IEnumerator Use() {
        holder.IncreaseHealth(5);
        yield return StartCoroutine(this.Destroy());
		yield break;
	}
}
