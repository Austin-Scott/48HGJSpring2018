using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReload : Card {

	public override IEnumerator Use() {
        //This card simply exists to be destroyed at the end of a turn
        DestroyAtEndOfTurn();
		return null;
	}

}
