using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShowOff : RangedCard {

	public override IEnumerator Use() {
        holder.IncreaseDexterity(1);
        target.IncreaseDexterity(-1);
		return null;
	}
}
