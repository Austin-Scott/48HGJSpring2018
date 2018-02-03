using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPhysicallyFit : MeleeCard {

	public override IEnumerator Use() {
        holder.IncreaseStrength(1);
        //TODO: Destroy this card
		return null;
	}

	public override void UpdateDamageText() {
		//TODO
	}
}
