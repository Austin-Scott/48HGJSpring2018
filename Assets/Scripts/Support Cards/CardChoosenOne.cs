using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChoosenOne : Card {

	public override IEnumerator Use() {
        //Support cards cost zero when this is in your hand
		return null;
	}
}
