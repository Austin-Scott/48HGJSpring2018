using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Basic card characteristics
public class Card : MonoBehaviour {

	/// Called when a card is used and placed on the board.
    public virtual IEnumerator Use() {
		yield break;
	}

	/// Number of phases a card executes over.
	[SerializeField]
	protected int cost;
	public int GetCost() { return cost; }

	/// Text mesh of the title.
	public TextMesh titleText;
	/// Text mesh of the cost display.
	public TextMesh costText;
	/// Text mesh of the text.
	public TextMesh text;

	/// Fetches all neccessary components for the card to use.
	public void GetAllComponents() {
		TextMesh[] textMeshes = GetComponentsInChildren<TextMesh>();
		titleText = textMeshes[0];
		costText = textMeshes[1];
		text = textMeshes[2];
	}

	/// The character using the card
	public DeckHolder holder { get; private set; }

	/// Position of the card on the board before it is moved for hover animation.
	Vector3 previousPosition;
	/// Rotation of the card on the board before it is moved by a hover animation.
	Quaternion previousRotation;

	/// List of all current movement coroutines
	List<IEnumerator> currentMovementCoroutines = new List<IEnumerator>();

	/// True when the card cannot be played or viewed.
	bool busy;

	/// True while the player is dragging a card
	bool grabbing = false;

	public bool onBoard = false;

	bool mousingOver = false;

	/// Hovers the card so the player can see it better.
	private IEnumerator Hover() {
		Vector3 desiredPosition = previousPosition + Vector3.up * 2f;
		IEnumerator hoverUp = LerpTransform(desiredPosition, Quaternion.identity, 2f);
		currentMovementCoroutines.Add(hoverUp);
		StartCoroutine(hoverUp);
		while (mousingOver) {
			yield return null;
		}
		StopCoroutine(hoverUp);
		currentMovementCoroutines.Remove(hoverUp);
		IEnumerator hoverDown = Return();
		currentMovementCoroutines.Add(hoverDown);
		yield return StartCoroutine(hoverDown);
	}

	/// Dehovers the card.
	private IEnumerator Return() {
		yield return StartCoroutine(LerpTransform(previousPosition, previousRotation, 2f));
	}

	/// Slerps the card's transform to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Transform desiredTransform, float speed = 1f) {
		yield return StartCoroutine(LerpTransform(desiredTransform.position, desiredTransform.rotation, speed));
	}

	/// Slerps the card's transofrm to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Vector3 desiredPosition, Quaternion desiredRotation, float speed = 1f) {
		while (transform.rotation != desiredRotation || transform.position != desiredPosition) {
			float deltaTime = Time.deltaTime * 5f * speed;
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, deltaTime);
			transform.position = Vector3.Lerp(transform.position, desiredPosition, deltaTime);
			if (Quaternion.Angle(transform.rotation, desiredRotation) < 10f) {
				transform.rotation = desiredRotation;
			}
			if (Vector3.Distance(transform.position, desiredPosition) < 0.2f) {
				transform.position = desiredPosition;
			}
			// transform.localScale = desiredTransform.localScale;//TODO scale almost finished scaling check (if we decide to use it)
			yield return null;
		}
	}

	// public void StartMoveCoroutine(IEnumerator movementCoroutine) {

	// }

	public IEnumerator Move(IEnumerator movementCoroutine, bool interuptable) {
		if (busy) {
			yield break;
		}
		if (currentMovementCoroutines.Count == 0) {
			previousPosition = transform.position;
			previousRotation = transform.rotation;
		} else {
			foreach (IEnumerator coroutine in currentMovementCoroutines) {
				StopCoroutine(coroutine);
			}
			// StopAllCoroutines();
		}
		busy = !interuptable;
		currentMovementCoroutines.Add(movementCoroutine);
		yield return StartCoroutine(movementCoroutine);
		currentMovementCoroutines.Remove(movementCoroutine);
		busy = false;
	}

	public IEnumerator Grab() {
		grabbing = true;
		while (grabbing) {
			// Stop grabbing if mouse up
			if (Input.GetMouseButtonUp(0)) {
				grabbing = false;
				break;
			}

			// this creates a horizontal plane passing through this object's center
			Plane plane = new Plane(Vector3.up, new Vector3(0f, 0.3f, 0f));
			// create a ray from the mousePosition
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// plane.Raycast returns the distance from the ray start to the hit point
			float distance;
			// I don't think this should ever return false.
			if (plane.Raycast(ray, out distance)){
				// some point of the plane was hit - get its coordinates
				Vector3 hitPoint = ray.GetPoint(distance);
				// use the hitPoint to position the card.
				// Move card to cursor's position on a 3d plane of the board.
				transform.position = hitPoint;
			}
			yield return null;
		}
		// TODO end of grab behavior
		yield return StartCoroutine(Return());
	}

	protected virtual void OnMouseOver() {
		/// Clicked on card
		if (Input.GetMouseButtonDown(0)) {
			StartCoroutine(Move(Grab(), false));
		}
	}

	protected virtual void OnMouseEnter() {
		mousingOver = true;
		/// If card is in hand and not busy, hover it so player can see it better.
		StartCoroutine(Move(Hover(), true));
	}

	protected virtual void OnMouseExit() {
		mousingOver = false;
	}

	/// Sets the cards friendy and target properties.
	public virtual void Initialize(DeckHolder holder) {
		this.holder = holder;
		// gameObject.SetActive(true);
	}

	private void ResetPosition() {
		transform.position = previousPosition;
		transform.rotation = previousRotation;
	}
}
