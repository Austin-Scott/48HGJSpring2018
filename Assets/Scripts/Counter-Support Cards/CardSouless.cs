using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSouless : Card {

	public override IEnumerator Use() {
        //TODO: Cannot sell soul when this is in your hand
		return null;
	}
}
