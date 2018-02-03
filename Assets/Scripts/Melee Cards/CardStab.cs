using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStab : MeleeCard {

	public override IEnumerator Use() {
        target.Damage(5);
        //TODO: If blocked destroy card
		return null;
	}

	public override void UpdateDamageText() {
		//TODO
	}
}
