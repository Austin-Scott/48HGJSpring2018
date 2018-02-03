using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPhysicallyFit : Card {

	public override IEnumerator Use() {
        holder.IncreaseStrength(1);
        //TODO: Destroy this card
		return null;
	}
}
