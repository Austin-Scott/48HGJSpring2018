using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGrab : MeleeCard {

	public override IEnumerator Use() {
        if (holder.GetStrength() > target.GetStrength())
        {
            //TODO: destroy all of target's block cards
        }
		return null;
	}

	public override void UpdateDamageText() {
		//TODO
	}
}
