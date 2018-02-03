using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPunch : MeleeCard {

	public override IEnumerator Use() {
        target.Damage(2);
        //TODO: Lose 2 health if the attack was blocked
		return null;
	}

	public override void UpdateDamageText() {
		//TODO
	}
}
