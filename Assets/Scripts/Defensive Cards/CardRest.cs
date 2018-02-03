using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRest : Card {

	public override IEnumerator Use() {
        holder.IncreaseHealth(2);
		return null;
	}
}
