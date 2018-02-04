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

	/// True if the card is on the board
	public bool onBoard = false;

	/// True if the card is in the deck
	public bool inDeck = true;

	/// Type of the card (not implemented)
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
	public Character holder { get; private set; }

	/// The character opposite the holder. Target
	public Character target { get; private set; }

	/// Position of the card on the board before it is moved for hover animation.
	Vector3 positionBeforeHover;

	/// The current hover or dehover routine.	
	Coroutine hoverCoroutine;

	/// True if the card is currently moving.
	bool moving = false;

	/// Although hovering is considered moving, it can be interrupted be re-hovering, so it needs to be stored.
	bool hovering = false;

	/// Although dehovering is considered moving, it can be interrupted be re-hovering, so it needs to be stored.
	bool dehovering = false;

	/// True while the player is dragging a card
	bool grabbing = false;

	public virtual IEnumerator Hover() {
		if (moving && !hovering) {
			yield break;
		}
		hovering = true;
		dehovering = false;
		if (hoverCoroutine != null) {
			transform.position = positionBeforeHover;
		} else {
			positionBeforeHover = transform.position;
		}
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			// deHovering = false;
		}
		hoverCoroutine = StartCoroutine(LerpPosition(transform.position + Vector3.up * 2f, 2f));
		yield return hoverCoroutine;
	}

	public virtual IEnumerator DeHover() {
		if (moving && !hovering) {
			yield break;
		}
		hovering = true;
		dehovering = true;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
		}
		hoverCoroutine = StartCoroutine(LerpPosition(positionBeforeHover, 2f));
		yield return hoverCoroutine;
		hovering = false;
		dehovering = false;
		hoverCoroutine = null;
	}

	public void DestroyAtEndOfTurn() {
		Board.endTurn += StartDestroyAnimation;
	}

	public virtual void StartDestroyAnimation() {
		StartCoroutine(Destroy());
	}

	public virtual IEnumerator Destroy() {
		Destroy(gameObject);
		yield break; //TODO destruction animation	
	}

	/// Slerps the card's transofrm to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Transform desiredTransform, float speed = 1f) {
		yield return StartCoroutine(LerpTransform(desiredTransform.position, desiredTransform.rotation, speed));
	}

	/// Slerps the card's transofrm to a given transform over time. Speed of 1 is default.
	private IEnumerator LerpTransform (Vector3 desiredPosition, Quaternion desiredRotation, float speed = 1f) {
		moving = true;
		while (transform.rotation != desiredRotation || transform.position != desiredPosition) {
			float deltaTime = Time.deltaTime * 5f * speed;
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, deltaTime);
			transform.position = Vector3.Slerp(transform.position, desiredPosition, deltaTime);
			if (Quaternion.Angle(transform.rotation, desiredRotation) < 5f) {
				transform.rotation = desiredRotation;
			}
			if (Vector3.Distance(transform.position, desiredPosition) < 0.1f) {
				transform.position = desiredPosition;
			}
			float rotationZ = transform.rotation.eulerAngles.z;
			if ((rotationZ >= 90 && rotationZ <= 270) || (rotationZ <= -90 && rotationZ >= -270)) {
				SetAllText(false);
			} else {
				SetAllText(true);
			}
			// transform.localScale = desiredTransform.localScale;//TODO scale almost finished scaling check (if we decide to use it)
			yield return null;
		}
		moving = false;
	}

	/// Moves the card to a certain position over time. Speed of 1 is default.
	private IEnumerator LerpPosition (Vector3 desiredPosition, float speed = 1f) {
		moving = true;
		while (transform.position != desiredPosition) {
			float deltaTime = Time.deltaTime * 5 * speed;
			transform.position = Vector3.Slerp(transform.position, desiredPosition, deltaTime);
			if (Vector3.Distance(transform.position, desiredPosition) < 0.1f) {
				transform.position = desiredPosition;
			}
			yield return null;
		}
		moving = false;
	}

	/// Smoothly move the card to desired position.
	public IEnumerator SmoothMove (Vector3 desiredPosition, float speed = 1f) {
		hovering = false;
		dehovering = false;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			transform.position = positionBeforeHover;
		}
		yield return StartCoroutine(LerpPosition(desiredPosition, speed));
	}

	/// Smoothly transform the card to the desired position.
	public IEnumerator SmoothTransform (Transform desiredTransform, float speed = 1f) {
		hovering = false;
		dehovering = false;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			transform.position = positionBeforeHover;
		}
		yield return StartCoroutine(LerpTransform(desiredTransform, speed));
	}

	/// Smoothly transform the card to the desired position.
	public IEnumerator SmoothTransform (Vector3 desiredPosition, Quaternion desiredRotation, float speed = 1f) {
		hovering = false;
		dehovering = false;
		if (hoverCoroutine != null) {
			StopCoroutine(hoverCoroutine);
			hoverCoroutine = null;
			transform.position = positionBeforeHover;
		}
		yield return StartCoroutine(LerpTransform(desiredPosition, desiredRotation, speed));
	}

	protected virtual void Awake() {

	}

	protected virtual void Start() {

	}

	protected virtual void Update() {
		if (grabbing && Input.GetMouseButtonUp(0)) {
			grabbing = false;
			//if not placed, put back in hand
			if (!onBoard) {
				StartCoroutine(holder.PositionHand());
			}
		}
		if (grabbing) {
			// this creates a horizontal plane passing through this object's center
			Plane plane = new Plane(new Vector3(0f, 0.3f, 0f), Vector3.up);
			// create a ray from the mousePosition
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// plane.Raycast returns the distance from the ray start to the hit point
			float distance;
			if (plane.Raycast(ray, out distance)){
				// some point of the plane was hit - get its coordinates
				Vector3 hitPoint = ray.GetPoint(distance);
				// use the hitPoint to position the card.
				transform.position = hitPoint;
				positionBeforeHover = transform.position;
			}
		}
	}

	protected virtual void OnMouseOver() {
		if ((hoverCoroutine == null || dehovering) && !grabbing && !onBoard) {
			StartCoroutine(Hover());
		}
		if (Input.GetMouseButtonDown(0) && (!moving || hovering) && !inDeck && holder == Board.player) {
			if (hoverCoroutine != null) {
				StopCoroutine(hoverCoroutine);
			}
			if (onBoard) {
				StartCoroutine(holder.AddCard(this));
			} else {
				grabbing = true;
			}
		}
	}

	// protected virtual void OnMouseEnter() {
	// 	if (!grabbing && !onBoard) {
	// 		StartCoroutine(Hover());
	// 	}
	// }

	protected virtual void OnMouseExit() {
		if (!grabbing && !onBoard) {
			StartCoroutine(DeHover());
		}
	}

	/// Sets the cards friendy and target properties.
	public virtual void Initialize(Character holder, Character target) {
		this.holder = holder;
		this.target = target;
	}

	void SetAllText(bool active) {
		titleText.gameObject.SetActive(active);
		text.gameObject.SetActive(active);
		costText.gameObject.SetActive(active);
	}
}
