using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBlock : Card {

	public override IEnumerator Use() {
        Shield.CreateShield(5, holder);
        //Add end of turn callBack to check if shield still exists
        Board.endTurn += onEndOfTurn;
		return null;
	}

    public void onEndOfTurn()
    {
        //TODO: Check if shield still exists
        Board.endTurn -= onEndOfTurn;
    }
}
