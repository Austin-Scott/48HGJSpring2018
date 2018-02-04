using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlan : Card {

	public override IEnumerator Use() {
        yield return holder.DrawCard();
        yield return holder.DrawCard();
        yield break;
	}
}
