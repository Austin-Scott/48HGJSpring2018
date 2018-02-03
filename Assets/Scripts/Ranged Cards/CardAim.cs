using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAim : Card {

	public override IEnumerator Use() {
        new DexterityBuff(3, holder);
        return null;
	}
}
