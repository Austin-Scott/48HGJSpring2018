using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CardType {
	Melee,
	Ranged,
	Defense,
	White,
	Black
};

public abstract class Card : MonoBehaviour {

	public abstract IEnumerator Use();

	/// Number of phases a card executes over.
	protected int cost;

	CardType cardType;

	/// Text mesh of the title.
	[SerializeField]
	protected TextMesh titleText;
	/// Text mesh of the cost display.
	[SerializeField]
	protected TextMesh costText;
	/// Text mesh of the text.
	[SerializeField]
	protected TextMesh text;

	/// The character using the card
	protected Character holder { get; private set; }

	/// The character opposite the holder. Target
	protected Character target { get; private set; }

	// public virtual IEnumerator Hover() {
	// 	//TODO move card closer to player infront of all other game elements.
	// }

	public virtual IEnumerator Destroy() {
		Destroy(gameObject);
		yield break; //TODO destruction animation	
	}

	/// Slerps the card's transofrm to a given transform over time. Speed of 1 is default.
	public IEnumerator LerpTransform (Transform desiredTransform, float speed = 1f) {
		while (transform != desiredTransform) {
			float deltaTime = Time.deltaTime * 5f * speed;
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredTransform.rotation, deltaTime);
			transform.position = Vector3.Slerp(transform.position, desiredTransform.position, deltaTime);
			transform.localScale = Vector3.Slerp(transform.localScale, desiredTransform.localScale, deltaTime);
			yield return null;
		}
	}

	/// Moves the card to a certain position over time. Speed of 1 is default.
	public IEnumerator LerpPosition (Vector3 desiredPosition, float speed = 1f) {
		while (transform.position != desiredPosition) {
			float deltaTime = Time.deltaTime * 5 * speed;
			transform.position = Vector3.Slerp(transform.position, desiredPosition, deltaTime);
			yield return null;
		}
		
	}

	public IEnumerator Flip(float speed = 1f) {
		yield break;
	}

	protected virtual void Awake() {

	}

	protected virtual void Start() {

	}
	
}
