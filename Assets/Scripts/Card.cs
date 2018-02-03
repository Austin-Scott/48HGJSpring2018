using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {

	public abstract IEnumerator Play();
	public abstract IEnumerator Use();

	/// Number of phases a card executes over.
	int cost;

	/// Text mesh of the title.
	TextMesh titleText;
	/// Text mesh of the title.
	TextMesh costText;
	/// Text mesh of the title.
	TextMesh text;

	/// Board the card is a part of.
	Board board;

	// public virtual IEnumerator Hover() {
	// 	//TODO move card closer to player infront of all other game elements.
	// }

	// public virtual IEnumerator Destroy() {
		
	// }

	void OnEnable() {

	}
	
}
