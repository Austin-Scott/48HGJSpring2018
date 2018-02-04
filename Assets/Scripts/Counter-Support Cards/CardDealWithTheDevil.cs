using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDealWithTheDevil : Card {

	public override IEnumerator Use() {
        Board.endPhase += CheckSoul;
		return null;
	}

    IEnumerable addSouless()
    {
        yield return holder.AddCard(GameController.CreateCard(typeof(CardSouless)));
        yield break;
    }

    public void CheckSoul()
    {
        //TODO
        // if(holder.soldSoul && !holder.hasCard(typeof(CardSouless)))
        // {
        //     addSouless();
        //     Board.endPhase -= CheckSoul;
        // }
    }
}
