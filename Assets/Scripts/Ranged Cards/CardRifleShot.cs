using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRifleShot : RangedCard {

	public override IEnumerator Use() {
        target.Damage(CalculateDamage(5));
        yield return holder.AddCard(GameController.CreateCard(typeof(CardReload)));
        yield break;
	}

	public override void UpdateDamageText() {
        text.text = "Deal (" + CalculateDamage(5) + ") ranged damage. Adds a reload to your hand.";
	}
}
