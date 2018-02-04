using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFranticThinking : Card {

	public override IEnumerator Use() {
        yield return holder.DiscardRandom();
        yield return holder.DiscardRandom();
        yield return holder.DrawCard();
        yield return holder.DrawCard();
        yield break;
	}
}
