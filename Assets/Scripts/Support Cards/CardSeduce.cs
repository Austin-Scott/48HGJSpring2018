using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSeduce : Card {

	public override IEnumerator Use() {
        yield return target.AddCard(GameController.CreateCard(typeof(CardReload)));
        yield break;
	}
}
