using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPsychUp : MeleeCard {

	public override IEnumerator Use() {
        holder.IncreaseStrength(3);
		return null;
	}
}
