﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOilWeapon : Card {

	public override IEnumerator Use() {
        holder.IncreaseDexterity(3);
        //TODO: Destroy this card
        return null;
	}
}
