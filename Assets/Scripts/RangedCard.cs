using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedCard : Card {

	public int CalculateDamage(int baseDamage) {
		return baseDamage + holder.GetDexterity();
	}
}
