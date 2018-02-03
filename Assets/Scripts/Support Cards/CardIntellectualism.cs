using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIntellectualism : Card {

	public override IEnumerator Use() {
        yield return holder.DrawCard();
        yield return holder.DrawCard();
        DestroyAtEndOfTurn();
        yield break;
	}
}
