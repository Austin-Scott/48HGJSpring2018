using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {

	public abstract IEnumerator Use();

	/// Number of phases a card executes over.
	protected int cost;

	/// Text mesh of the title.
	[SerializeField]
	TextMesh titleText;
	/// Text mesh of the cost display.
	[SerializeField]
	TextMesh costText;
	/// Text mesh of the text.
	[SerializeField]
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

	public IEnumerator LerpTransform (Transform desiredTransform) {
		while (transform != desiredTransform) {
			float deltaTime = Time.deltaTime;
			transform.rotation = Quaternion.Lerp(transform.rotation, desiredTransform.rotation, deltaTime);
			transform.position = Vector3.Lerp(transform.position, desiredTransform.position, deltaTime);
			transform.localScale = Vector3.Lerp(transform.localScale, desiredTransform.localScale, deltaTime);
			yield return null;
		}
		Debug.Log("There");
	}

	public IEnumerator LerpPosition (Vector3 desiredPosition) {
		while (transform.position != desiredPosition) {
			float deltaTime = Time.deltaTime*5;
			transform.position = Vector3.Slerp(transform.position, desiredPosition, deltaTime);
			yield return null;
		}
		Debug.Log("There");
	}
	
}
