using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAdrenalineRush : MeleeCard {

	public override IEnumerator Use() {
		yield break;
	}

	public override void UpdateDamageText() {
		text.text = "Everytime you take damage to health this turn, gain 2 strength this turn. Destroy this card.";
	}
}
