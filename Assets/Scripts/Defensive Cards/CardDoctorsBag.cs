using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDoctorsBag : Card {

	public override IEnumerator Use() {
        holder.IncreaseHealth(int.MaxValue);
        DestroyAtEndOfTurn();
        return null;
	}
}
