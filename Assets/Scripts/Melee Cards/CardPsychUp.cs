using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPsychUp : Card {

	public override IEnumerator Use() {
        holder.IncreaseStrength(3);
		yield break;
	}
}
