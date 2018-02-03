using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHalberdStrike : MeleeCard {

	public override IEnumerator Use() {
        target.Damage(15);
        //TODO: make this attack unable to be blocked
		return null;
	}
}
