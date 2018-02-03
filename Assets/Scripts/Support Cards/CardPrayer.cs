using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrayer : Card {

	public override IEnumerator Use() {
        yield return holder.AddCard(GameController.CreateCard(typeof(CardReload)));
        yield break;
	}
}
