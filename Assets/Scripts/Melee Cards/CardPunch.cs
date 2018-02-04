using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPunch : MeleeCard {

	public override IEnumerator Use() {
        target.Damage(CalculateDamage(2));
        if(target.getTotalShield()>0)
        {
            holder.Damage(2);
        }
		yield break;
	}

	public override void UpdateDamageText() {
        text.text = "Deal (" + CalculateDamage(2) + ") damage.\nLose 2 health if blocked.";
	}
}
