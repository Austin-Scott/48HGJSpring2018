using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTerrify : Card {

	public override IEnumerator Use() {
        yield return target.AddCard(GameController.CreateCard(typeof(CardTerrify)));
        yield break;
	}
}
