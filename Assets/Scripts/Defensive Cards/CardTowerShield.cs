using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTowerShield : Card {

	public override IEnumerator Use() {
        Shield.CreateShield(10, holder);
		return null;
	}
}
