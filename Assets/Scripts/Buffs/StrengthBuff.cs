using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthBuff : Buff {

	int amount;
	Character character;

	public StrengthBuff (int amount, Character character) {
		this.amount = amount;
		this.character = character;
		character.IncreaseStrength(amount);
		Board.endTurn += Revert;
	}

	public override void Revert () {
		character.IncreaseStrength(-amount);
		Board.endTurn -= Revert;
	}
}
