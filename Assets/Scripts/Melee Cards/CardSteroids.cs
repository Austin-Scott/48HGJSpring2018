using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSteroids : Card {

	public override IEnumerator Use() {
        holder.IncreaseStrength(3);
        DestroyAtEndOfTurn();
		return null;
	}
}
