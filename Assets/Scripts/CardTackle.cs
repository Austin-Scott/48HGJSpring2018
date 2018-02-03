using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTackle : MeleeCard {

	public override IEnumerator Use() {
        //TODO: make this attack unable to be blocked
        target.Damage(5);
		return null;
	}
}
