using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArmor : Card {

	public override IEnumerator Use() {
        Shield.CreateShield(3, holder);
        //Add callback
        Board.endTurn += onEndOfTurn;
		return null;
	}

    public void onEndOfTurn()
    {
        //TODO: check if shield still exists
        Board.endTurn -= onEndOfTurn;
    }
}
