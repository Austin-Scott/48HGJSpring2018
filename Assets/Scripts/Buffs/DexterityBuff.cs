using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexterityBuff : Buff {

	int amount;
	Character character;

	public DexterityBuff (int amount, Character character) {
		this.amount = amount;
		this.character = character;
		character.IncreaseDexterity(amount);
		Board.endTurn += Revert;
	}

	public override void Revert () {
		character.IncreaseDexterity(-amount);
		Board.endTurn -= Revert;
	}
}
