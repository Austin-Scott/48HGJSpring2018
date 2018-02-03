using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {

	public abstract IEnumerator Use();

	/// Number of phases a card executes over.
	protected int cost;

	/// Text mesh of the title.
	TextMesh titleText;
	/// Text mesh of the title.
	TextMesh costText;
	/// Text mesh of the title.
	TextMesh text;

	/// The character using the card
	protected Character holder { get; private set; }

	/// The character opposite the holder. Target
	protected Character target { get; private set; }

	// public virtual IEnumerator Hover() {
	// 	//TODO move card closer to player infront of all other game elements.
	// }

	// public virtual IEnumerator Destroy() {
		
	// }

	// /// Sets a card on the board during the planning phase.
	// public virtual IEnumerator Set(int phaseIndex) {

	// }
	
}
