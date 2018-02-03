using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPistolShot : RangedCard {

	public override IEnumerator Use() {
        target.Damage(CalculateDamage(2));
        yield return holder.AddCard(GameController.CreateCard(typeof(CardReload)));
        yield break;
	}

	public override void UpdateDamageText() {
        text.text = "Deal (" + CalculateDamage(2) + ") ranged damage. Adds a reload to your hand.";
	}
}
