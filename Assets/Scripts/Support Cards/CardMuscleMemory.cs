using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMuscleMemory : Card {

	public override IEnumerator Use() {
        //TODO: the next time you play a card for the second time that costs 2 or more reduce its cost by 1
        yield return StartCoroutine(this.Destroy());
        yield break;
	}
}
