using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTackle : MeleeCard {

	public override IEnumerator Use() {
        target.DamageToHealth(CalculateDamage(5));
        holder.DamageToHealth(CalculateDamage(5));
		return null;
	}

	public override void UpdateDamageText() {
        text.text = "Deal (" + CalculateDamage(5) + ") damage to both players. Cannot be blocked.";
	}
}
